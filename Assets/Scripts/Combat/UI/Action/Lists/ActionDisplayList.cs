using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Lists;

namespace Levels.Combat {
    
    public class ActionDisplayList : InventoryUI<ICombatAction>
    {
        [SerializeField] private CombatLevelActionUIController combatLevelActionUIController;
        [SerializeField] private ActionExecutionUI actionExecutionUIPrefab;
        private ActionSelectUI actionSelectUI;
        public void Start() {
            actionSelectUI = GlobalUtils.getComponentInHeirarchy<ActionSelectUI>(transform);
        }
        public override void leftClick(int index)
        {
            combatLevelActionUIController.displayExecutionUI(actionExecutionUIPrefab,actionSelectUI.CreatureCombatObject,elements[index]);
        }

        public override void rightClick(int index)
        {
            
        }
    }
}

