using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
    public abstract class Equipment : Lootable, IItem
    {
        [SerializeField] private string id;
        [SerializeField] private Rarity rarity;
        [SerializeField] private Sprite sprite;
        public Rarity Rarity {get => rarity;}
        public string Id {get => id;}
        public abstract int modifyType(CreatureStat type, int val);

        public override Sprite getSprite()
        {
            return sprite;
        }
    }

    public enum CreatureStat {
        Attack,
        Speed,
        Ability,
        Armor,
        Health
    }

    public enum Rarity {
        Common,
        Rare,
        Exotic,
        Legendary
    }
}

