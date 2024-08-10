using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Inventory;
using Items;
using Player;
using Crafting.Recipes;

namespace Crafting.UI {
    public abstract class RecipeInputList<T> : MonoBehaviour where T : Recipe
    {
        public virtual void display(T recipe) {
            GlobalUtils.deleteChildren(transform);
            /*
            List<ItemSlot> items = recipe.getInputs();
            foreach (ItemSlot itemSlot in items) {
                displayItemSlot(itemSlot);
            }
            */
        }
        public abstract StackableItemSlotUI getSlotPrefab(ItemSlot itemSlot);
        public virtual StackableItemSlotUI displayItemSlot(ItemSlot itemSlot) {
            StackableItemSlotUI itemSlotUI = GameObject.Instantiate(getSlotPrefab(itemSlot));
            itemSlotUI.transform.SetParent(transform,false);
            float amount = PlayerIO.Instance.getAmountOfLootable(itemSlot);
            //itemSlotUI.displayWithRequirement(itemSlot,(int)amount);
            return itemSlotUI;
        }
    }
}

