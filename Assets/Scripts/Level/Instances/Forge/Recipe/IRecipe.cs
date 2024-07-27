using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

namespace Crafting.Recipes {
    public interface IRecipe
    {
        public List<ItemSlot> getInputs();
        public ItemSlot getOutput();
    }

    public abstract class Recipe : ScriptableObject, IRecipe, IDisplayable
    {
        public abstract string getName();
        public abstract Sprite getSprite();
        public abstract int getAmount();
        public abstract List<ItemSlot> getInputs();
        public abstract ItemSlot getOutput();
    }
    public class SelectableItemSlot : ItemSlot {
        public TieredItemType tieredItemType;
        public Rarity selectionRarity;

        public SelectableItemSlot(Lootable lootable, int amount, TieredItemType tieredItemType, Rarity rarity) : base(lootable, amount)
        {
            this.selectionRarity = rarity;
            this.tieredItemType = tieredItemType;
        }
    }
}

