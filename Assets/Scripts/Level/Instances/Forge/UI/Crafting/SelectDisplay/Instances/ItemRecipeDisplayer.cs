using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crafting.Recipes;
using Items;
using Player;

namespace Crafting.UI {
    public class ItemRecipeDisplayer : RecipeDisplayer<ItemRecipe>
    {
        public override bool canCraft()
        {
            PlayerIO playerIO = PlayerIO.Instance;
            return true;
        }

        protected override void executeCraft()
        {
            //PlayerIO.Instance.give(displayedRecipe.getOutput());
        }

        protected override void displayExtra(ItemRecipe recipe)
        {
            
        }
    }
}

