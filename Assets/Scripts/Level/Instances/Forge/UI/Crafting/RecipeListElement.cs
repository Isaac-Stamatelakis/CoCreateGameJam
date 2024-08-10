using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Inventory;
using Crafting.Recipes;
using UnityEngine.UI;
using TMPro;
using Items;

namespace Crafting.UI {
    public class RecipeListElement : UIInventoryDisplayer<Recipe>
    {
        //[SerializeField] private StackableItemSlotUI itemSlotUI;
        [SerializeField] private TextMeshProUGUI nameText;
        private RecipeUIController recipeUIController;
        public override void display(Recipe element)
        {
            //itemSlotUI.display(element.getSprite(),0);
            nameText.text = element.getName();
        }
    }
}

