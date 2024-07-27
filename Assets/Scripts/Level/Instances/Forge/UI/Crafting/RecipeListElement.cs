using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Inventory;
using Crafting.Recipes;
using UnityEngine.UI;
using TMPro;
using Items;

namespace Crafting.UI {
    public class RecipeListElement : ClickableUIDisplayer<Recipe>
    {
        [SerializeField] private ItemSlotUI itemSlotUI;
        [SerializeField] private TextMeshProUGUI nameText;
        private RecipeUIController recipeUIController;
        private int index;
        public override void display(Recipe element, InventoryUI<Recipe> inventory, int index)
        {
            itemSlotUI.display(element.getSprite(),element.getAmount());
            nameText.text = element.getName();
            this.index = index;
            this.recipeUIController = (RecipeUIController) inventory;
            

        }

        public override void leftClick()
        {
            recipeUIController.selectRecipe(index);
            Debug.Log(index);
        }

        public override void rightClick()
        {
            
        }
    }
}

