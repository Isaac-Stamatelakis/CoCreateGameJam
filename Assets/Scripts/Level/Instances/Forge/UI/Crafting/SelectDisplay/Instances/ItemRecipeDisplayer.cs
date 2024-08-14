using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crafting.Recipes;
using Items;
using UI.Inventory;
using Player;

namespace Crafting.UI {
    public class ItemRecipeDisplayer : RecipeDisplayer<ItemRecipe>
    {
        [SerializeField] private InventoryUI<RequirementIntItemSlot> recipeInputList;
        [SerializeField] protected StackableItemSlotUI outputItem;
        protected ItemRecipe itemRecipe;
        public override void craft()
        {
            if (!PlayerIO.Instance.hasItems(itemRecipe.getIntItemSlotInputs())) {
                craftFail();
                return;
            }
            craftSucceed();
            PlayerIO.Instance.take(itemRecipe.getIntItemSlotInputs());
            PlayerIO.Instance.give(itemRecipe.Output);
            recipeInputList.display(ItemSlotUtils.toRequirementSlots(itemRecipe.getIntItemSlotInputs()),null);
            
        }
        public override void display(ItemRecipe recipe)
        {
            this.itemRecipe = recipe;
            recipeInputList.display(ItemSlotUtils.toRequirementSlots(recipe.getIntItemSlotInputs()),null);
            outputItem.display(recipe.Output);
            outputItem.setTheme(null);
        }
    }
}

