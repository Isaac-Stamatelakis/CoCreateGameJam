using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;

namespace Levels.Combat {
    public class CombatLevelController : MonoBehaviour
    {
        [SerializeField] private CombatLevelUIController uiController;
        [SerializeField] private CombatCreatureContainer humanPlayerCreatures;
        [SerializeField] private CombatCreatureContainer aiPlayerCreatures;
        private List<CreatureInCombat> creatureTurns;
        private CreatureCombatObject currentlyHighlightedCreature;
        private CombatPlayer humanPlayer;
        private CombatPlayer aiPlayer;
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
            uiController.DisplayedCreatureUI.display(creatureTurns[0]);
            creatureTurns[0].CreatureCombatObject.highlight(HighlightType.Turn);
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
            if (creatureTurns.Count > 0) {
                CreatureInCombat currentCreatureTurn = creatureTurns[0];
                bool selectedCreatureWhoseTurnItIs = creatureCombatObject.CreatureInCombat.Equals(currentCreatureTurn);
                if (selectedCreatureWhoseTurnItIs) {
                    if (currentlyHighlightedCreature != null) {
                        currentlyHighlightedCreature.highlight(null);
                    }
                    uiController.DisplayedCreatureUI.display(creatureTurns[0]);
                    currentlyHighlightedCreature = null;
                    return;
                }
            }
            if (creatureCombatObject.Equals(currentlyHighlightedCreature)) {
                currentlyHighlightedCreature.highlight(null);
                uiController.DisplayedCreatureUI.display(creatureTurns[0]);
                currentlyHighlightedCreature = null;
                return;
            }
            creatureCombatObject.highlight(HighlightType.View);
            uiController.DisplayedCreatureUI.display(creatureCombatObject.CreatureInCombat);
            if (currentlyHighlightedCreature != null) {
                currentlyHighlightedCreature.highlight(null);
            }
            currentlyHighlightedCreature = creatureCombatObject;
        }
    }
}

