using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Actions.Script;

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

        public void displaySelect(CreatureCombatObject creatureCombatObject, CombatPlayer combatPlayer) {
            toggleDisplay(actionSelectUI.gameObject);
            ActionSelectUI.display(creatureCombatObject,combatPlayer.Consumables);
        }

        public void displayExecutionUI(ActionExecutionUI actionExecutionUIPrefab, CreatureCombatObject creatureCombatObject, ICombatAction combatAction) {
            toggleDisplay(null);
            ActionExecutionUI actionExecutionUI = GameObject.Instantiate(actionExecutionUIPrefab);
            actionExecutionUI.gameObject.SetActive(true);
            actionExecutionUI.transform.SetParent(transform,false);
            actionExecutionUI.display(combatAction,creatureCombatObject,this);
        }

        public void displayEnemyTurn(CreatureInCombat creatureInCombat, ScriptedAction scriptedAction) {
            toggleDisplay(enemyTurnUI.gameObject);
            enemyTurnUI.display(creatureInCombat.EquipedCreeture,scriptedAction);
        }
    }
}

