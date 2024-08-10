using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
using UI.Inventory;
using Crafting.Recipes;

namespace Crafting.UI {
    public class SelectableRecipeInputList : RecipeInputList<TierUpgradeRecipe>
    {
        [SerializeField] private StackableItemSlotUI staticPrefab;
        [SerializeField] private SelectableItemSlotUI selectablePrefab;
        private List<UniqueItemSlot> selectableItemSlots;
        private List<int> selectedIndexs;

        private void clickCallback(int index) {
            selectedIndexs.Add(index);
            UniqueItemSlot selectableItemSlot = selectableItemSlots[index];
            
        }
        private void clearClickCallback(int index) {
            selectedIndexs.Remove(index);
            UniqueItemSlot selectableItemSlot = selectableItemSlots[index];
        }

        public override StackableItemSlotUI getSlotPrefab(ItemSlot itemSlot)
        {
            if (itemSlot is UniqueItemSlot) {
                return selectablePrefab;
            }
            return staticPrefab;
        }

        public override StackableItemSlotUI displayItemSlot(ItemSlot itemSlot)
        {
            StackableItemSlotUI itemSlotUI = base.displayItemSlot(itemSlot);
            if (itemSlot is UniqueItemSlot selectableItemSlot) {
                selectableItemSlots.Add(selectableItemSlot);
                //((SelectableItemSlotUI) itemSlotUI).sync(clickCallback,clearClickCallback,selectableItemSlots.Count-1);
            }
            return itemSlotUI;
        }

        public override void display(TierUpgradeRecipe recipe)
        {
            selectableItemSlots = new List<UniqueItemSlot>();
            selectedIndexs = new List<int>();
            base.display(recipe);
        }
    }
}

