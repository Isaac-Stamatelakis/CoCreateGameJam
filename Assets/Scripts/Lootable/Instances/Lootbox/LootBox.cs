using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Trading;

namespace LootBoxes {
    
    public abstract class LootBox : Lootable
    {
        public Sprite sprite;
        public LootBoxType type;
        [Header("Lootables that can be won\nChance is frequency/sum of all frequencies")]
        public List<LootFrequency> loot;
        public override Sprite getSprite()
        {
            return sprite;
        }

        public LootableCount open() {
            int totalFrequency = 0;
            foreach (LootFrequency lootable in loot) {
                totalFrequency += lootable.frequency;
            }
            int ran = Random.Range(0,totalFrequency);
            totalFrequency = 0;
            foreach (LootFrequency lootable in loot) {
                totalFrequency += lootable.frequency;
                if (totalFrequency > ran) {
                    return new LootableCount(lootable.val,lootable.amount);
                }
            }
            return null;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    [System.Serializable]
    public class LootFrequency {
        public Lootable val;
        public int frequency;
        public float amount = 1;
    }

    public class LootboxCount : IDisplayable, ILootableCount {
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

        public string getName()
        {
            return lootBox.name;
        }

        public float getAmount()
        {
            return (float) count;
        }

        public void addAmount(float amount)
        {
            count += (int)amount;
        }

        public Lootable getLootable()
        {
            return lootBox;
        }
    }

}
