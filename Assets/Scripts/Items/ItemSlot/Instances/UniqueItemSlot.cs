using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
    public class UniqueItemSlot : ItemSlot
    {
        protected Rarity rarity;
        public UniqueItemSlot(Lootable lootable, Rarity rarity) : base(lootable)
        {
            this.rarity = rarity;
        }
    }
}

