using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crafting.Recipes;
using Items;

namespace Crafting.UI {
    public interface IRecipeDisplayer {
        public bool canCraft();
        public void craft();
    }
    public abstract class RecipeDisplayer<T> : MonoBehaviour, IRecipeDisplayer where T : Recipe
    {
        [SerializeField] private RecipeInputList<T> recipeInputList;
        [SerializeField] protected StackableItemSlotUI outputItem;
        protected T displayedRecipe;
        public RecipeInputList<T> RecipeInputList { get => recipeInputList; }
        public void display(T recipe) {
            this.displayedRecipe = recipe;
            RecipeInputList.display(recipe);
            //outputItem.displayWithRequirement(recipe.getOutput(),10);
            //displayExtra(recipe);
        }

        protected abstract void displayExtra(T recipe);
        public abstract bool canCraft();
        protected abstract void executeCraft();
        public void craft() {
            if (!canCraft()) {
                return;
            }
            executeCraft();
        }
    }

}
