using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crafting.Recipes;

namespace Crafting.UI {
    public class TierUpgradeRecipeDisplayer : RecipeDisplayer<TierUpgradeRecipe>
    {
        public override bool canCraft()
        {
            throw new System.NotImplementedException();
        }

        protected override void executeCraft()
        {
            throw new System.NotImplementedException();
        }

        protected override void displayExtra(TierUpgradeRecipe recipe)
        {
            
        }
    }
}

