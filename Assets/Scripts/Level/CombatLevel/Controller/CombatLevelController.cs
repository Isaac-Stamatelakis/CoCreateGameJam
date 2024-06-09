using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Creatures.Actions;
using Actions.Script;

namespace Levels.Combat {
    public class CombatLevelController : MonoBehaviour
    {
        [SerializeField] private CombatLevelUIController uiController;
        [SerializeField] private CombatCreatureContainer humanPlayerCreatures;
        [SerializeField] private CombatCreatureContainer aiPlayerCreatures;
        private List<CreatureInCombat> creatureTurns;
        private CombatPlayer humanPlayer;
        private CombatPlayer aiPlayer;
        private CreatureHighlightController creatureHighlightController;
        public CreatureHighlightController CreatureHighlightController { get => creatureHighlightController; }

        public void load(CombatPlayer humanPlayer, CombatPlayer aiPlayer, CombatLevel combatLevel) {
            this.humanPlayer = humanPlayer;
            humanPlayerCreatures.displayCreatures(humanPlayer.Creatures);
            this.aiPlayer = aiPlayer;
            aiPlayerCreatures.displayCreatures(aiPlayer.Creatures);
            List<CombatPlayer> players = new List<CombatPlayer>{
                humanPlayer,
                aiPlayer
            };
            creatureTurns = CombatLevelFactory.generateTurns(players);
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
                Debug.Log("AI Win");
                return;
            }
            if (aiPlayer.IsDead()) {
                Debug.Log("Player Win");
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
                StartCoroutine(moveAI());
                return;
            }
            Debug.LogError($"{currentCreatureTurn.name} Doesn't belong to the player or ai");
        }

        public IEnumerator nextCreatureTurn() {
            CreatureInCombat currentTurn = creatureTurns[0];
            yield return currentTurn.CreatureCombatObject.resetPosition();
            creatureTurns.RemoveAt(0);
            creatureTurns.Add(currentTurn);
            while (creatureTurns.Count > 0 && creatureTurns[0].IsDead) {
                creatureTurns.RemoveAt(0);
            }
            handleNewCreatureTurn();
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
            List<ScriptedAction> actions = currentCreatureTurn.CreatureInCombat.EquipedCreeture.creeture.Actions;
            if (actions.Count == 0) {
                Debug.LogWarning($"{currentCreatureTurn.name} has no actions");
                yield return null;;
            }
            int ran = Random.Range(0,actions.Count);
            ScriptedAction chosenAction = actions[ran];
            CommandExecutionState commandExecutionState = new CommandExecutionState(chosenAction,currentCreatureTurn);
            yield return StartCoroutine(commandExecutionState.executeSection());
            Debug.Log(commandExecutionState.Complete);
            while (!commandExecutionState.Complete) {
                CreatureSelector creatureSelector = commandExecutionState.getCurrentSelector();
                switch (creatureSelector.TargetType) {
                    case TargetType.Ally:
                        ran = Random.Range(0,aiPlayer.Creatures.Count);
                        creatureSelector.Creatures.Add(aiPlayer.Creatures[ran].CreatureCombatObject);
                        break;
                    case TargetType.Enemy:
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

