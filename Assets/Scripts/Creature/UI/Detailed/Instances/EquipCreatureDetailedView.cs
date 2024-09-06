using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Creatures.UI {
    public class EquipCreatureDetailedView : CreatureDetailedDisplay
    {
        
        protected override void addListeners()
        {
            
        }

        protected override void onDismiss()
        {
            CreatureEquipUI creatureEquipUI = GlobalUtils.getComponentInHeirarchy<CreatureEquipUI>(transform);
            creatureEquipUI.ParentToRefresh.refresh();
            GameObject.Destroy(creatureEquipUI.gameObject);
        }
    }
}

