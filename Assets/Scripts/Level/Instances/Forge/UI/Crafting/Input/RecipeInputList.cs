using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Inventory;
using Items;
using Crafting.Recipes;

namespace Crafting.UI {
    public abstract class RecipeInputList<T> : MonoBehaviour where T : Recipe
    {
        public virtual void display(T recipe) {
            GlobalUtils.deleteChildren(transform);
            List<ItemSlot> items = recipe.getInputs();
            foreach (ItemSlot itemSlot in items) {
                displayItemSlot(itemSlot);
            }
        }
        public abstract ItemSlotUI getSlotPrefab(ItemSlot itemSlot);
        public virtual ItemSlotUI displayItemSlot(ItemSlot itemSlot) {
            ItemSlotUI itemSlotUI = GameObject.Instantiate(getSlotPrefab(itemSlot));
            itemSlotUI.transform.SetParent(transform,false);
            itemSlotUI.display(itemSlot);
            return itemSlotUI;
        }
    }
}

