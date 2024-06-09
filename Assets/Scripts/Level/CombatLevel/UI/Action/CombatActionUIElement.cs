using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels.Combat {
    public class CombatActionUIElement : ActionUIElement
    {
        public override void leftClick()
        {
            if (inventory is not ICombatActionDisplayList combatActionDisplayList) {
                Debug.LogWarning("Combat Action UI Element did not belong to ICombatActionDisplayList");
                return;
            }
            combatActionDisplayList.showExecutionUI(combatAction);
        }

        public override void rightClick()
        {
            
        }
    }
}

