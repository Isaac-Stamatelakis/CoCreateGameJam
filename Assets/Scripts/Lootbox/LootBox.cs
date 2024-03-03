using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreatureModule;
using CurrencyModule;

namespace LootBoxModule {
    [CreateAssetMenu(fileName = "New Lootbox", menuName = "Lootbox")]
    public class LootBox : Lootable
    {
        public Sprite sprite;
        public LootBoxType type;
        public string id;
        [Header("Lootables that can be won\nChance is frequency/sum of all frequencies")]
        public List<LootFrequency> loot;

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

    public class LootboxCount {
        public LootboxCount(LootBox lootBox, int count) {
            this.lootBox = lootBox;
            this.count = count;
        }
        public LootBox lootBox;
        public int count;
    }

}
