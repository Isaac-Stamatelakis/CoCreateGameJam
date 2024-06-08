using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Creatures.Actions;

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
            uiController.ActionUIController.displaySelect(creatureTurns[0],humanPlayer);
            creatureHighlightController.setCurrentCreatureTurn(creatureTurns[0].CreatureCombatObject);
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
    }
}

