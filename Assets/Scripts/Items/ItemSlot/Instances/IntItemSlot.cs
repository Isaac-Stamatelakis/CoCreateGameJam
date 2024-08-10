using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
    [System.Serializable]
    public class IntItemSlot : StackableItemSlot
    {
        private int amount;
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

        public override void merge(StackableItemSlot stackableItemSlot)
        {
            if (stackableItemSlot is not IntItemSlot intItemSlot) {
                return;
            }
            amount += intItemSlot.Amount;
        }
    }
}

