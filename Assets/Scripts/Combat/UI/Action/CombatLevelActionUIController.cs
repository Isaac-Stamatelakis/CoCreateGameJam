using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;

namespace Levels.Combat {
    public class CombatLevelActionUIController : MonoBehaviour
    {
        [SerializeField] private CombatLevelUIController combatLevelUIController;
        [SerializeField] private ActionSelectUI actionSelectUI;
        [SerializeField] private EnemyTurnUI enemyTurnUI;
        private GameObject currentlyDisplayed;
        public ActionSelectUI ActionSelectUI { get => actionSelectUI;}
        public CombatLevelUIController CombatLevelUIController { get => combatLevelUIController;  }

        private void toggleDisplay(GameObject toDisplay) {
            if (currentlyDisplayed != null) {
                currentlyDisplayed.SetActive(false);
            }
            if (toDisplay != null) {
                toDisplay.gameObject.SetActive(true);
            }
            currentlyDisplayed = toDisplay;
            
            
        }

        public void displaySelect(CreatureInCombat creatureInCombat, CombatPlayer combatPlayer) {
            toggleDisplay(actionSelectUI.gameObject);
            ActionSelectUI.display(creatureInCombat,combatPlayer.Consumables);
        }

        public void displayExecutionUI(ActionExecutionUI actionExecutionUIPrefab, ICombatAction combatAction) {
            toggleDisplay(null);
            ActionExecutionUI actionExecutionUI = GameObject.Instantiate(actionExecutionUIPrefab);
            actionExecutionUI.gameObject.SetActive(true);
            actionExecutionUI.transform.SetParent(transform,false);
            actionExecutionUI.display(combatAction,this);
        }

        public void displayEnemyTurn(CreatureInCombat creatureInCombat) {
            toggleDisplay(enemyTurnUI.gameObject);
            enemyTurnUI.display(creatureInCombat.EquipedCreeture);
        }
    }
}

