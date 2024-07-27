using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Inventory;

namespace Levels.Combat {
    public interface ICombatActionDisplayList {
        public void showExecutionUI(ICombatAction combatAction);
    }
    public class ActionDisplayList : InventoryUI<ICombatAction>, ICombatActionDisplayList
    {
        [SerializeField] private CombatLevelActionUIController combatLevelActionUIController;
        [SerializeField] private ActionExecutionUI actionExecutionUIPrefab;
        public void showExecutionUI(ICombatAction combatAction)
        {
            combatLevelActionUIController.displayExecutionUI(actionExecutionUIPrefab,combatAction);
        }
    }
}

