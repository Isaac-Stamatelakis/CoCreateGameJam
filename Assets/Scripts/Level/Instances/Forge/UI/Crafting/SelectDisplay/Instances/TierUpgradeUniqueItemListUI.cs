using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Lists;
using Items;

namespace Crafting.UI {
    public class TierUpgradeUniqueItemListUI : InventoryUI<UniqueItemSlot>
    {
        private TierUpgradeRecipeDisplayer tierUpgradeRecipeDisplayer;
        public void Start() {
            tierUpgradeRecipeDisplayer = GlobalUtils.getComponentInHeirarchy<TierUpgradeRecipeDisplayer>(transform);
        }
        public override void leftClick(int index)
        {
            
        }

        public override void rightClick(int index)
        {
            
        }
    }
}

