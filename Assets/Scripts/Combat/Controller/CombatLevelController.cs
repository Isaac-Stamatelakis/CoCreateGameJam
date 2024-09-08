using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Actions;
using Actions.Script;
using Actions.MCTS;
using Items.Equipment;

namespace Levels.Combat {
    public class CombatLevelController : MonoBehaviour
    {
        private static CombatLevelController instance;
        public void Awake() {
            instance = this;
        }
        [SerializeField] private CombatLevelUIController uiController;
        [SerializeField] private CombatCreatureContainer humanPlayerCreatures;
        [SerializeField] private CombatCreatureContainer aiPlayerCreatures;
        [SerializeField] private GameOverController playerLoseUIPrefab;
        [SerializeField] private PlayerWinUI playerWinUIPrefab;
        [SerializeField] private Transform spawnedObjectContainer;
        private readonly int CURRENT_TURN_Z_CHANGE = 1;
        private CreatureMoveOrder creatureMoveOrder;
        private CombatPlayer humanPlayer;
        private CombatPlayer aiPlayer;
        private CreatureHighlightController creatureHighlightController;
        public CreatureHighlightController CreatureHighlightController { get => creatureHighlightController; }
        public static CombatLevelController Instance { get => instance; }
        public Transform CanvasTransform { get => uiController.transform; }
        public CreatureMoveOrder CreatureMoveOrder {get => creatureMoveOrder;}
        public Transform SpawnedObjectContainer {get => spawnedObjectContainer;}
        private MonteCarloTreeSearch monteCarloTreeSearch;
        public void load(CombatPlayer humanPlayer, CombatPlayer aiPlayer, CombatLevelObject combatLevel) {
            if (humanPlayer.Creatures.Count == 0) {
                // TODO CHANGE TO SPECIAL SCREEN TELLING PLAYER THEY HAVE NO CREATURES
                showGameOverScreen(playerLoseUIPrefab);
            }
            this.humanPlayer = humanPlayer;
            this.aiPlayer = aiPlayer;
            creatureMoveOrder = new CreatureMoveOrder(humanPlayer,aiPlayer);

            creatureHighlightController = new CreatureHighlightController(humanPlayer,uiController.DisplayedCreatureUI);
            initalizePlayerObjects(humanPlayer,humanPlayerCreatures);
            initalizePlayerObjects(aiPlayer,aiPlayerCreatures);

            handleNewCreatureTurn();
        }

        private void initalizePlayerObjects(CombatPlayer combatPlayer, CombatCreatureContainer combatCreatureContainer) {
            List<CreatureInCombat> combatCreatures = CreatureMoveOrder.getCombatCreatures(combatPlayer);
            combatCreatureContainer.displayCreatures(CreatureMoveOrder.getCombatCreatures(combatPlayer));
            foreach (CreatureInCombat creatureInCombat in combatCreatures) {
                uiController.CombatCreatureUIContainer.addCreature(creatureInCombat.CreatureCombatObject);
            }
        }

        public IEnumerator specialActionCoRoutine(CreatureCombatObject selfCreature, CreatureCombatObject target, SpecialSelectTarget specialSelectTarget) {
            yield return StartCoroutine(specialAction(selfCreature,target,specialSelectTarget));
        }

        public IEnumerator specialAction(CreatureCombatObject selfCreature, CreatureCombatObject target, SpecialSelectTarget specialSelectTarget) {
            if (selfCreature != null && target != null) {
                foreach (EnchantedEquipment enchantedEquipment in selfCreature.getEquipment()) {
                    if (enchantedEquipment == null || enchantedEquipment.Equipment == null) {
                        continue;
                    }
                    EquipmentActionCollection equipmentActionCollection = ActionRegistry.getInstance().getAction<EquipmentActionCollection>(enchantedEquipment.getId());
                    if (equipmentActionCollection == null) {
                        continue;
                    }
                    ScriptedAction scriptedAction = SpecialSelectCommandUtils.getAction(equipmentActionCollection,specialSelectTarget);
                    if (scriptedAction == null) {
                        continue;
                    }
                    LiveCommandExecutionState commandExecutionState = new LiveCommandExecutionState(scriptedAction,selfCreature,false);
                    yield return StartCoroutine(commandExecutionState.executeSection());
                    while (!commandExecutionState.Complete) {
                        commandExecutionState.CreatureSelector = new CreatureSelector<CreatureCombatObject>(new List<CreatureCombatObject>{target});
                        yield return StartCoroutine(commandExecutionState.executeSection());
                    }
                    yield return null;
                }
            }
            
        }

        public void handleNewCreatureTurn() {
            if (humanPlayer.IsDead()) {
                showGameOverScreen(playerLoseUIPrefab);
                return;
            }
            if (aiPlayer.IsDead()) {
                showGameOverScreen(playerWinUIPrefab);
                return;
            }
            creatureHighlightController.setSelector(null);
            CreatureCombatObject currentCreatureTurn = creatureMoveOrder.getCurrentCombatCreature().CreatureCombatObject;
            Vector3 currentTurnPosition = currentCreatureTurn.transform.position;
            currentTurnPosition.z -= CURRENT_TURN_Z_CHANGE;
            currentCreatureTurn.transform.position = currentTurnPosition;

            creatureHighlightController.setCurrentCreatureTurn(currentCreatureTurn);
            if (humanPlayer.HasCreature(currentCreatureTurn)) {
                uiController.ActionUIController.displaySelect(currentCreatureTurn,humanPlayer);
                return;
            } else {
                StartCoroutine(moveAI());
            }
        }

        public IEnumerator nextCreatureTurn() {
            creatureMoveOrder.clearDeadCreatures();
            if (creatureMoveOrder.IsEmpty) {
                showGameOverScreen(playerLoseUIPrefab);
                Debug.Log("Tie");
                yield break;
            }
            CreatureCombatObject currentCreatureTurn = creatureMoveOrder.getCurrentCombatCreature().CreatureCombatObject;
            if (currentCreatureTurn != null) {
                Vector3 currentTurnPosition = currentCreatureTurn.transform.position;
                currentTurnPosition.z += CURRENT_TURN_Z_CHANGE;
                currentCreatureTurn.transform.position = currentTurnPosition;
                yield return currentCreatureTurn.resetPosition();
            }
            creatureMoveOrder.changeTurn();
            Debug.Log(creatureMoveOrder.Creatures[0].getName());
            handleNewCreatureTurn();
        }

        private void showGameOverScreen(GameOverController prefab) {
            GameOverController instantiated = GameObject.Instantiate(prefab);
            instantiated.transform.SetParent(uiController.transform,false);
            ActionRegistry.getInstance().freeAll();
        }

        public void Update() {
            if (Input.GetMouseButtonDown(0)) {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                raycastCreatureObjects(mousePosition);
            }
        }
        private void raycastCreatureObjects(Vector2 mousePosition) {
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("Creature"));
            if (hit.collider == null) {
                return;
            }
            CreatureCombatObject creatureCombatObject = hit.collider.GetComponent<CreatureCombatObject>();
            if (creatureCombatObject == null) {
                return;
            }
            CreatureHighlightController.highlight(creatureCombatObject);
        }

        private IEnumerator moveAI() {
            CreatureCombatObject currentCreatureTurn = creatureMoveOrder.getCurrentCombatCreature().CreatureCombatObject;
            CreatureActionCollection creatureActionCollection = ActionRegistry.getInstance().getAction<CreatureActionCollection>(currentCreatureTurn.CreatureInCombat.EquipedCreeture.creeture.Id);
            List<ScriptedAction> actions = creatureActionCollection.actions;
            if (actions.Count == 0) {
                Debug.LogWarning($"{currentCreatureTurn.name} has no actions");
                yield break;
            }
            /*
            int ran = Random.Range(0,actions.Count);
            ScriptedAction chosenAction = actions[ran];

            CreatureSelector creatureSelector = commandExecutionState.getCurrentSelector();
            switch (creatureSelector.TargetType) {
                case CreatureSelectionType.Ally:
                    ran = Random.Range(0,aiPlayer.Creatures.Count);
                    EquipedCreature creature = humanPlayer.Creatures[ran];
                    CreatureInCombat creatureInCombat = creatureMoveOrder.getCombatCreature(creature);
                    creatureSelector.Creatures.Add(creatureInCombat.CreatureCombatObject);
                    break;
                case CreatureSelectionType.Enemy:
                    ran = Random.Range(0,humanPlayer.Creatures.Count);
                    EquipedCreature creature1 = humanPlayer.Creatures[ran];
                    CreatureInCombat creatureInCombat1 = creatureMoveOrder.getCombatCreature(creature1);
                    creatureSelector.Creatures.Add(creatureInCombat1.CreatureCombatObject);
                    break;
            }
            */
            
            MonteCarloTreeSearch monteCarloTreeSearch = new MonteCarloTreeSearch();
            GameMove gameMove = monteCarloTreeSearch.getMove(new GameState(creatureMoveOrder),100);
            
            uiController.ActionUIController.displayEnemyTurn(currentCreatureTurn.CreatureInCombat,gameMove.ScriptedAction);
            LiveCommandExecutionState commandExecutionState = new LiveCommandExecutionState(gameMove.ScriptedAction,currentCreatureTurn,true);
            // Pre Selection
            yield return StartCoroutine(commandExecutionState.executeSection());
            // Selection
            ManualCreatureSelector creatureSelector = commandExecutionState.getManualCreatureSelector();
            if (creatureSelector != null) {
                List<CreatureCombatObject> combatObjects = new List<CreatureCombatObject>();
                foreach (CreatureInCombat creatureInCombat in gameMove.Selector.getCreatures()) {
                    combatObjects.Add(creatureInCombat.CreatureCombatObject);
                }
                creatureSelector.Creatures = combatObjects;
            }
            yield return StartCoroutine(commandExecutionState.executeSection());

            // Post Selection
            yield return StartCoroutine(commandExecutionState.executeSection());
            yield return StartCoroutine(nextCreatureTurn());
        }
    }
}

