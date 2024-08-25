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
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private TextMeshProUGUI nameText;
        private RecipeUIController recipeUIController;
        public override void display(Recipe element)
        {
            image.sprite = element.getSprite();
            text.text = element.getSubText();
            nameText.text = element.getName();
        }
    }
}

