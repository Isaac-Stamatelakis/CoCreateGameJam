using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
    public class DoubleItemSlot : StackableItemSlot
    {
        private double amount;
        public DoubleItemSlot(Lootable lootable, double amount) : base(lootable)
        {
            this.amount = amount;
        }

        public double Amount { get => amount; set => amount = value; }

        public override double amountToDouble()
        {
            return amount;
        }

        public override int amountToInt()
        {
            return (int) amount;
        }

        public override string amountToString()
        {
            return $"{amount}:F1";
        }

        public override StackableItemSlot clone()
        {
            return new DoubleItemSlot(
                this.Lootable,
                this.amount
            );
        }

        public override void merge(StackableItemSlot stackableItemSlot)
        {
            if (stackableItemSlot is not DoubleItemSlot doubleItemSlot) {
                return;
            }
            amount += doubleItemSlot.amount;
        }

        public override void subtractDouble(double value)
        {
            amount -= value;
        }

        public override void subtractInt(int value)
        {
            amount -= (double) value;
        }
    }
}

