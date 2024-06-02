using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using CurrencyModule;

namespace LootBoxes {
    
    public abstract class LootBox : Lootable
    {
        public Sprite sprite;
        public LootBoxType type;
        public string id;
        [Header("Lootables that can be won\nChance is frequency/sum of all frequencies")]
        public List<LootFrequency> loot;

        public override Sprite getSprite()
        {
            return sprite;
        }

        public Lootable open() {
            int totalFrequency = 0;
            foreach (LootFrequency lootable in loot) {
                totalFrequency += lootable.frequency;
            }
            int ran = Random.Range(0,totalFrequency);
            totalFrequency = 0;
            foreach (LootFrequency lootable in loot) {
                totalFrequency += lootable.frequency;
                if (totalFrequency > ran) {
                    return lootable.val;
                }
            }
            return null;
        }
    }

    [System.Serializable]
    public class LootFrequency {
        public Lootable val;
        public int frequency;
    }

    public class LootboxCount : IDisplayable {
        public LootboxCount(LootBox lootBox, int count) {
            this.lootBox = lootBox;
            this.count = count;
        }
        public LootBox lootBox;
        public int count;

        public Sprite getSprite()
        {
            return lootBox.sprite;
        }
    }

}
