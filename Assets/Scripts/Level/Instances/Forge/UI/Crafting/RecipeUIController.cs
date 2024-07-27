using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Inventory;
using Items;
using Crafting.Recipes;
using UnityEngine.UI;
using TMPro;

namespace Crafting.UI {
    public class RecipeUIController : InventoryUI<Recipe>
    {
        [SerializeField] private DetailedRecipeDisplayController recipeDisplayController;
        private int currentlySelect;
        public void selectRecipe(int index) {
            if (currentlySelect == index) {
                return;
            }
            currentlySelect = index;
            recipeDisplayController.display(elements[index]);
        }
    }
}

