using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreatureModule;
using CurrencyModule;

namespace LootBoxModule {
    [CreateAssetMenu(fileName = "New Lootbox", menuName = "Lootbox")]
    public class LootBox : ScriptableObject
    {
        public Sprite sprite;
        public Animation animation;
        [Header("Lootables that can be won\nChance is frequency/sum of all frequencies")]
        public List<LootFrequency<Creeture>> creatures;
        public List<LootFrequency<Currency>> currencies;

        public ILootable open() {
            List<ILootable> loot = new List<ILootable>();
            int totalFrequency = 0;
            foreach (LootFrequency<Creeture> creature in creatures) {
                totalFrequency += creature.frequency;
            }
            foreach (LootFrequency<Currency> currency in currencies) {
                totalFrequency += currency.frequency;
            }
            int ran = Random.Range(0,totalFrequency);
            totalFrequency = 0;
            foreach (LootFrequency<Creeture> creature in creatures) {
                totalFrequency += creature.frequency;
                if (totalFrequency > ran) {
                    return creature.val;
                }
            }
            foreach (LootFrequency<Currency> currency in currencies) {
                totalFrequency += currency.frequency;
                if (totalFrequency > ran) {
                    return currency.val;
                }
            }
            return null;
        }
    }

    [System.Serializable]
    public class LootFrequency<Loot> where Loot : ILootable {
        public Loot val;
        public int frequency;
    }

}
