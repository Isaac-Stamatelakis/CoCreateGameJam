using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;

namespace Levels.Combat {
    public class CombatLevelActionUIController : MonoBehaviour
    {
        [SerializeField] private CombatLevelUIController combatLevelUIController;
        [SerializeField] private ActionSelectUI actionSelectUI;

        public ActionSelectUI ActionSelectUI { get => actionSelectUI;}
        public CombatLevelUIController CombatLevelUIController { get => combatLevelUIController;  }

        public void displaySelect(CreatureInCombat creatureInCombat, CombatPlayer combatPlayer) {
            ActionSelectUI.gameObject.SetActive(true);
            ActionSelectUI.display(creatureInCombat,combatPlayer.Consumables);
        }

        public void displayExecutionUI(ActionExecutionUI actionExecutionUIPrefab, ICombatAction combatAction) {
            ActionSelectUI.gameObject.SetActive(false);
            ActionExecutionUI actionExecutionUI = GameObject.Instantiate(actionExecutionUIPrefab);
            actionExecutionUI.transform.SetParent(transform,false);
            actionExecutionUI.display(combatAction,this);
        }
    }
}

