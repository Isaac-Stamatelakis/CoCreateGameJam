using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Trading;
using Items;

namespace LootBoxes {
    
    public abstract class LootBox : Lootable
    {
        public Sprite sprite;
        public LootBoxType type;
        [Header("Lootables that can be won\nChance is frequency/sum of all frequencies")]
        public LootTableObject lootTable;
        
        public override Sprite getSprite()
        {
            return sprite;
        }

        public List<IntItemSlot> open() {
            return LootTableUtils.openLootTable(lootTable);
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
