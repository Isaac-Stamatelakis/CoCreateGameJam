using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryModule {
    [CreateAssetMenu(fileName = "New Equipment", menuName = "Item/Equipment")]
    public class Equipment : ScriptableObject, IItem
    {
        public string id;
        public EquipmentType type;
        public Rarity rarity;
        public Sprite sprite;
    }

    public enum EquipmentType {
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

