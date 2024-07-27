using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
    public abstract class Equipment : Lootable, ISecondaryHeaderDisplayable
    {
        [SerializeField] private Rarity rarity;
        [SerializeField] private Sprite sprite;
        public Rarity Rarity {get => rarity;}
        public string Id {get => id;}
        public abstract int modifyType(CreatureStat type, int val);

        public override Sprite getSprite()
        {
            return sprite;
        }
        public string getSecondaryHeader()
        {
            return rarity.ToString();
        }
    }

    public static class EquipmentFactory {
        public static List<string> serialize(List<Equipment> equipmentList) {
            List<string> equipmentIds = new List<string>();
            foreach (Equipment equipment in equipmentList) {
                equipmentIds.Add(equipment.Id);
            }
            return equipmentIds;
        }
    }

    public enum CreatureStat {
        Attack,
        Speed,
        Ability,
        Armor,
        Health,
        MaxMana,
        InitalMana
    }

    public enum Rarity {
        Common,
        Rare,
        Exotic,
        Legendary
    }
}

