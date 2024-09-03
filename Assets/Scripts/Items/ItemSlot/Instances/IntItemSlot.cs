using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
    [System.Serializable]
    public class IntItemSlot : StackableItemSlot
    {
        [SerializeField] private int amount;
        public IntItemSlot(Lootable lootable, int amount) : base(lootable)
        {
            this.amount = amount;
        }
        public int Amount { get => amount; set => amount = value;}

        public override double amountToDouble()
        {
            return (double) amount;
        }

        public override int amountToInt()
        {
            return amount;
        }

        public override string amountToString()
        {
            return amount.ToString();
        }

        public override StackableItemSlot clone()
        {
            return new IntItemSlot(
                this.Lootable,
                this.amount
            );
        }

        public override void merge(StackableItemSlot stackableItemSlot)
        {
            if (stackableItemSlot is not IntItemSlot intItemSlot) {
                return;
            }
            amount += intItemSlot.Amount;
        }

        public override void subtractDouble(double value)
        {
            amount -= (int) value;
        }

        public override void subtractInt(int value)
        {
            amount -= value;
        }
    }
}

