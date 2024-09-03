using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.Equipment {
    [System.Serializable]
    public class EnchantedEquipment : LootableAggregate
    {
        [SerializeField] private Equipment equipment;
        public override Lootable getLootable()
        {
            return equipment;
        }
        public EquipmentEnchant enchant;

        public Equipment Equipment { get => equipment; }

        public EnchantedEquipment(Equipment equipment, EquipmentEnchant enchant)
        {
            this.equipment = equipment;
            this.enchant = enchant;
        }

        public void modifyStat(ref int value, CreatureStat creatureStat) {

        }
    }
}
