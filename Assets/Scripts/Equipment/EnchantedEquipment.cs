using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.Equipment {
    public class EnchantedEquipment : LootableAggregate
    {
        private Equipment equipment;
        public override Lootable getLootable()
        {
            return equipment;
        }
        public EquipmentEnchant enchant;
        public EnchantedEquipment(Equipment equipment, EquipmentEnchant enchant)
        {
            this.equipment = equipment;
            this.enchant = enchant;
        }

        public void modifyStat(ref int value, CreatureStat creatureStat) {

        }
    }
}
