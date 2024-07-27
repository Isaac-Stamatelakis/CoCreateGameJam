using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crafting.Recipes;
using System;

namespace Crafting.UI {
    public class DetailedRecipeDisplayController : MonoBehaviour
    {
        [SerializeField] private Transform inputContentContainer;
        [SerializeField] private ItemRecipeDisplayer itemRecipeDisplayer;
        [SerializeField] private TierUpgradeRecipeDisplayer tierUpgradeRecipeDisplayer;
        private IRecipeDisplayer currentDisplayer;

        public IRecipeDisplayer CurrentDisplayer { get => currentDisplayer; }

        public void display(Recipe recipe) {
            GlobalUtils.deleteChildren(inputContentContainer.transform);
            GameObject recipeDisplayer = GameObject.Instantiate(getRecipeDisplayerPrefab(recipe));
            recipeDisplayer.transform.SetParent(inputContentContainer,false);
            // Probalby another way to do this 
            currentDisplayer = recipeDisplayer.GetComponent<IRecipeDisplayer>();
            Debug.Log(currentDisplayer==null);
            if (recipe is TierUpgradeRecipe tierUpgradeRecipe) {
                recipeDisplayer.GetComponent<TierUpgradeRecipeDisplayer>().display(tierUpgradeRecipe);
            } else if (recipe is ItemRecipe itemRecipe) {
                recipeDisplayer.GetComponent<ItemRecipeDisplayer>().display(itemRecipe);
            }
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


    }
}

