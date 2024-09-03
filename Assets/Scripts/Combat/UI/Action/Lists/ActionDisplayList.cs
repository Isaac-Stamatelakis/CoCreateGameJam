using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Lists;

namespace Levels.Combat {
    
    public class ActionDisplayList : InventoryUI<ICombatAction>
    {
        [SerializeField] private CombatLevelActionUIController combatLevelActionUIController;
        [SerializeField] private ActionExecutionUI actionExecutionUIPrefab;

        public override void leftClick(int index)
        {
            combatLevelActionUIController.displayExecutionUI(actionExecutionUIPrefab,elements[index]);
        }

        public override void rightClick(int index)
        {
            
        }
    }
}

