using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Actions;
using Actions.Script;

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
        private List<CreatureInCombat> creatureTurns;
        private CombatPlayer humanPlayer;
        private CombatPlayer aiPlayer;
        private CreatureHighlightController creatureHighlightController;
        public CreatureHighlightController CreatureHighlightController { get => creatureHighlightController; }
        public static CombatLevelController Instance { get => instance; }
        public Transform CanvasTransform { get => uiController.transform; }

        public void load(CombatPlayer humanPlayer, CombatPlayer aiPlayer, CombatLevel combatLevel) {
            this.humanPlayer = humanPlayer;
            humanPlayerCreatures.displayCreatures(humanPlayer.Creatures);
            this.aiPlayer = aiPlayer;
            aiPlayerCreatures.displayCreatures(aiPlayer.Creatures);
            List<CombatPlayer> players = new List<CombatPlayer>{
                humanPlayer,
                aiPlayer
            };
            creatureTurns = CombatLevelUtils.generateTurns(players);
            foreach (CreatureInCombat creatureInCombat in creatureTurns) {
                uiController.CombatCreatureUIContainer.addCreature(creatureInCombat.CreatureCombatObject);
            }
            creatureHighlightController = new CreatureHighlightController(humanPlayer,uiController.DisplayedCreatureUI);
            handleNewCreatureTurn();
        }

        public CreatureCombatObject getCurrentlyMovingCreature() {
            return creatureTurns[0].CreatureCombatObject;
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
            if (creatureTurns.Count < 0) {
                Debug.LogWarning("Tried to display creature turn of empty turn schedule");
                return;
            }
            creatureHighlightController.setSelector(null);
            CreatureCombatObject currentCreatureTurn = getCurrentlyMovingCreature();
            creatureHighlightController.setCurrentCreatureTurn(currentCreatureTurn);
            if (humanPlayer.Creatures.Contains(currentCreatureTurn.CreatureInCombat)) {
                uiController.ActionUIController.displaySelect(creatureTurns[0],humanPlayer);
                return;
            }
            if (aiPlayer.Creatures.Contains(currentCreatureTurn.CreatureInCombat)) {
                uiController.ActionUIController.displayEnemyTurn(creatureTurns[0]);
                StartCoroutine(moveAI());
                return;
            }
            Debug.LogError($"{currentCreatureTurn.name} Doesn't belong to the player or ai");
        }

        

        public IEnumerator nextCreatureTurn() {
            clearCreatureListOfDead(creatureTurns);
            clearCreatureListOfDead(humanPlayer.Creatures);
            clearCreatureListOfDead(aiPlayer.Creatures);
            if (creatureTurns.Count == 0) {
                showGameOverScreen(playerLoseUIPrefab);
                Debug.Log("Tie");
                yield return null;
            }
            CreatureInCombat currentTurn = creatureTurns[0];
            yield return currentTurn.CreatureCombatObject.resetPosition();
            creatureTurns.RemoveAt(0);
            creatureTurns.Add(currentTurn);
            while (creatureTurns.Count > 0 && creatureTurns[0].IsDead) {
                creatureTurns.RemoveAt(0);
            }
            handleNewCreatureTurn();
        }

        private void showGameOverScreen(GameOverController prefab) {
            GameOverController instantiated = GameObject.Instantiate(prefab);
            instantiated.transform.SetParent(uiController.transform,false);
            CreatureActionRegistry.getInstance().free();
        }

        private void clearCreatureListOfDead(List<CreatureInCombat> creatures) {
            for (int i = 0; i < creatures.Count; i++) {
                if (creatures[i].IsDead) {
                    creatures.RemoveAt(i);
                    i--;
                }
            }
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
            CreatureCombatObject currentCreatureTurn = getCurrentlyMovingCreature();
            CreatureActionCollection creatureActionCollection = CreatureActionRegistry.getInstance().getAction(currentCreatureTurn.CreatureInCombat.EquipedCreeture.creeture.Id);
            List<ScriptedAction> actions = creatureActionCollection.actions;
            if (actions.Count == 0) {
                Debug.LogWarning($"{currentCreatureTurn.name} has no actions");
                yield return null;;
            }
            int ran = Random.Range(0,actions.Count);
            ScriptedAction chosenAction = actions[ran];
            CommandExecutionState commandExecutionState = new CommandExecutionState(chosenAction,currentCreatureTurn);
            yield return StartCoroutine(commandExecutionState.executeSection());
            while (!commandExecutionState.Complete) {
                CreatureSelector creatureSelector = commandExecutionState.getCurrentSelector();
                switch (creatureSelector.TargetType) {
                    case CreatureSelectionType.Ally:
                        ran = Random.Range(0,aiPlayer.Creatures.Count);
                        creatureSelector.Creatures.Add(aiPlayer.Creatures[ran].CreatureCombatObject);
                        break;
                    case CreatureSelectionType.Enemy:
                        ran = Random.Range(0,humanPlayer.Creatures.Count);
                        creatureSelector.Creatures.Add(humanPlayer.Creatures[ran].CreatureCombatObject);
                        break;
                }
                yield return StartCoroutine(commandExecutionState.executeSection());
            }
            yield return StartCoroutine(nextCreatureTurn());

        }
    }
}

