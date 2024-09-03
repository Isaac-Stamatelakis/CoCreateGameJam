using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crafting.Recipes;
using System;
using UI.Lists;
using TMPro;
using UnityEngine.UI;

namespace Crafting.UI {
    public class DetailedRecipeDisplayController : UIInventoryDisplayer<Recipe>
    {
        [SerializeField] private Transform inputContentContainer;
        [SerializeField] private Button craftButton;
        [SerializeField] private ItemRecipeDisplayer itemRecipeDisplayer;
        [SerializeField] private TierUpgradeRecipeDisplayer tierUpgradeRecipeDisplayer;
        [SerializeField] private TextMeshProUGUI recipeTitle;
        private IRecipeDisplayer currentDisplayer;
        public IRecipeDisplayer CurrentDisplayer { get => currentDisplayer; }
        public void Start() {
            craftButton.onClick.AddListener(() => {
                currentDisplayer.craft();
            });
        }
        private GameObject getRecipeDisplayerPrefab<T>(T recipe) where T : Recipe {
            if (recipe is TierUpgradeRecipe) {
                return tierUpgradeRecipeDisplayer.gameObject;
            } else if (recipe is ItemRecipe) {
                return itemRecipeDisplayer.gameObject;
            } else {
                throw new System.Exception($"{recipe} was not covered by detailed recipe display controller");
            }
        }

        public override void display(Recipe recipe)
        {
            GlobalUtils.deleteChildren(inputContentContainer.transform);
            GameObject recipeDisplayer = GameObject.Instantiate(getRecipeDisplayerPrefab(recipe));
            recipeTitle.text = recipe.getName();
            recipeDisplayer.transform.SetParent(inputContentContainer,false);
            recipeDisplayer.GetComponent<IRecipeDisplayer>().setButton(craftButton);
            // Probalby another way to do this 
            currentDisplayer = recipeDisplayer.GetComponent<IRecipeDisplayer>();
            if (recipe is TierUpgradeRecipe tierUpgradeRecipe) {
                recipeDisplayer.GetComponent<TierUpgradeRecipeDisplayer>().display(tierUpgradeRecipe);
            } else if (recipe is ItemRecipe itemRecipe) {
                recipeDisplayer.GetComponent<ItemRecipeDisplayer>().display(itemRecipe);
            }
        }
    }
}

