using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crafting.Recipes;
using Items;

namespace Crafting.UI {
    public class StaticRecipeInputList : RecipeInputList<ItemRecipe>
    {
        [SerializeField] private ItemSlotUI itemSlotUIPrefab;
        public override ItemSlotUI getSlotPrefab(ItemSlot itemSlot)
        {
            return itemSlotUIPrefab;
        }
    }
}

