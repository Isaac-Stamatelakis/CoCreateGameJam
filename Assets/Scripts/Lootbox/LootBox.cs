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
        public List<LootFrequency> loot;
        public int rolls = 1;
        public bool repetitions = true;
        public override Sprite getSprite()
        {
            return sprite;
        }

        public List<IntItemSlot> open() {
            int totalFrequency = 0;
            foreach (LootFrequency lootable in loot) {
                totalFrequency += lootable.frequency;
            }
            
            List<IntItemSlot> lootedItemSlots = new List<IntItemSlot>();
            List<LootFrequency> temp = new List<LootFrequency>();
            foreach (LootFrequency lootFrequency in loot) {
                temp.Add(lootFrequency);
            }
            while (temp.Count > 0 && lootedItemSlots.Count < rolls) {
                int ran = Random.Range(0,totalFrequency);
                totalFrequency = 0;
                for (int i = 0; i < temp.Count; i++) {
                    LootFrequency lootFrequency = temp[i];
                    totalFrequency += lootFrequency.frequency;
                    if (totalFrequency > ran) {
                        lootedItemSlots.Add(ItemSlotUtils.fromLootFrequency(lootFrequency));
                        if (!repetitions) {
                            temp.RemoveAt(i);
                        }
                        break;
                    }
                    
                }
            }
            return lootedItemSlots;
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

    [System.Serializable]
    public class LootFrequency {
        public Lootable val;
        public int frequency;
        public int amount = 1;
    }
}
