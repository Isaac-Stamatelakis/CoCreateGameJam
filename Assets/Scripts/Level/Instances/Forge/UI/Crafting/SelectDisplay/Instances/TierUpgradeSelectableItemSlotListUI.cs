using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Inventory;
using Items;
using UnityEngine.UI;
using TMPro;

namespace Crafting.UI {
    public class TierUpgradeSelectableItemSlotListUI : InventoryUI<UniqueItemSlot>
    {
        public override void leftClick(int index)
        {
            Debug.Log("HI");
        }

        public override void rightClick(int index)
        {
            
        }
    }
}

