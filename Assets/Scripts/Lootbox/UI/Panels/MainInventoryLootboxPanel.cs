using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootBoxes;
using UnityEngine.UI;

namespace UI.Inventory {
    public class MainInventoryLootboxPanel : LootBoxPanel, IPanelDisplay
    {
        [SerializeField] private Image panel;
        public Color getColor()
        {
            return panel.color;
        }

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

        public void setPanelColor(Color color)
        {
            panel.color = color;
        }
    }
}

