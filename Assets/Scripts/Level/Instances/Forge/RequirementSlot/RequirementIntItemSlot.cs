using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

namespace Crafting {
    public class RequirementIntItemSlot : IntItemSlot
    {
        private int currentAmount;
        public RequirementIntItemSlot(Lootable lootable, int amount, int currentAmount) : base(lootable, amount)
        {
            this.currentAmount = currentAmount;
        }

        public int CurrentAmount { get => currentAmount;}
    }
}

