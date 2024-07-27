using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Inventory {
    public class CreatureInventoryDisplayUI : CreatureUIDisplay
    {
        public override void leftClick()
        {
            if (inventory is not IDetailedViewDisplayer detailedViewDisplayer) {
                Debug.LogWarning("Tried to display detailed view on non detailed view displayer");
                return;
            }
            detailedViewDisplayer.displayDetailedView(index);
            inventory.highlightSlot(index);
        }

        public override void rightClick()
        {
            
        }
    }
}

