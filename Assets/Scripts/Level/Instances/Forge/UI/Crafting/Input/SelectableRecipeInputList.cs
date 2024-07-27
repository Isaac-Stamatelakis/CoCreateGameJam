using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
using UI.Inventory;
using Crafting.Recipes;

namespace Crafting.UI {
    public class SelectableRecipeInputList : RecipeInputList<TierUpgradeRecipe>
    {
        [SerializeField] private ItemSlotUI staticPrefab;
        [SerializeField] private SelectableItemSlotUI selectablePrefab;
        private List<SelectableItemSlot> selectableItemSlots;
        private List<int> selectedIndexs;

        private void clickCallback(int index) {
            selectedIndexs.Add(index);
            SelectableItemSlot selectableItemSlot = selectableItemSlots[index];
            
        }
        private void clearClickCallback(int index) {
            selectedIndexs.Remove(index);
            SelectableItemSlot selectableItemSlot = selectableItemSlots[index];
        }

        public override ItemSlotUI getSlotPrefab(ItemSlot itemSlot)
        {
            if (itemSlot is SelectableItemSlot) {
                return selectablePrefab;
            }
            return staticPrefab;
        }

        public override ItemSlotUI displayItemSlot(ItemSlot itemSlot)
        {
            ItemSlotUI itemSlotUI = base.displayItemSlot(itemSlot);
            if (itemSlot is SelectableItemSlot selectableItemSlot) {
                selectableItemSlots.Add(selectableItemSlot);
                ((SelectableItemSlotUI) itemSlotUI).sync(clickCallback,clearClickCallback,selectableItemSlots.Count-1);
            }
            return itemSlotUI;
        }

        public override void display(TierUpgradeRecipe recipe)
        {
            selectableItemSlots = new List<SelectableItemSlot>();
            selectedIndexs = new List<int>();
            base.display(recipe);
        }
    }
}

