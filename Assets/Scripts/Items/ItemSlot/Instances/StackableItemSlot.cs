using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
    public interface IStackableItemSlot : IDisplayable {
        public string amountToString();
        public double amountToDouble();
        public int amountToInt();
    }
    public abstract class StackableItemSlot : ItemSlot, IStackableItemSlot
    {
        protected StackableItemSlot(Lootable lootable) : base(lootable)
        {

        }
        public abstract void merge(StackableItemSlot stackableItemSlot);
        public abstract string amountToString();
        public abstract double amountToDouble();
        public abstract int amountToInt();
        public abstract void subtractInt(int value);
        public abstract void subtractDouble(double value);
        public abstract StackableItemSlot clone();
    }
}

