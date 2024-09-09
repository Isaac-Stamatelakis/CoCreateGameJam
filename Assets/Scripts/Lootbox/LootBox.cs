using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Trading;
using Items;
using System.Threading.Tasks;

namespace LootBoxes {
    
    public abstract class LootBox : Lootable
    {
        public Sprite sprite;
        public LootBoxType type;
        public LootTableObject lootTableObject;
        public override Sprite getSprite()
        {
            return sprite;
        }

        public List<IntItemSlot> open() {
            return LootTableUtils.openLootTable(lootTableObject);
        }

        public override string ToString()
        {
            return base.ToString();
        }
        public override ItemSlotType getItemSlotType()
        {
            return ItemSlotType.Int;
        }
    }

    
}
