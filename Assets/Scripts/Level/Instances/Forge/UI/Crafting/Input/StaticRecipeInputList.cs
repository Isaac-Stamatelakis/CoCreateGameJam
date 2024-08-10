using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crafting.Recipes;
using Items;

namespace Crafting.UI {
    public class StaticRecipeInputList : RecipeInputList<ItemRecipe>
    {
        [SerializeField] private StackableItemSlotUI itemSlotUIPrefab;
        public override StackableItemSlotUI getSlotPrefab(ItemSlot itemSlot)
        {
            return itemSlotUIPrefab;
        }
    }
}

