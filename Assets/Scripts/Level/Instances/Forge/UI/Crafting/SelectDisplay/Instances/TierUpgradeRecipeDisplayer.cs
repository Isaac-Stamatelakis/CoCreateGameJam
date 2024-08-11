using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crafting.Recipes;
using UI.Inventory;
using Items;

namespace Crafting.UI {
    public class TierUpgradeRecipeDisplayer : RecipeDisplayer<TierUpgradeRecipe>
    {
        [SerializeField] private InventoryUI<IntItemSlot> recipeInputList;
        public override void craft()
        {
            throw new System.NotImplementedException();
        }

        public override void display(TierUpgradeRecipe element)
        {
            recipeInputList.display(element.getIntItemSlotInputs());
        }
    }
}

