using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        private int getEquipmentStat(Func<Equipment, int> statSelector)
        {
            if (equipment == null)
            {
                return 0;
            }
            return statSelector(equipment);
        }
        public int getAttack()
        {
            return getEquipmentStat(e => e.Attack);
        }

        public int getDefense()
        {
            return getEquipmentStat(e => e.Defense);
        }

        public int getAbility()
        {
            return getEquipmentStat(e => e.Ability);
        }

        public int getSpeed()
        {
            return getEquipmentStat(e => e.Speed);
        }

        public int getHealth()
        {
            return getEquipmentStat(e => e.Health);
        }

        public int getMaxMana()
        {
            return getEquipmentStat(e => e.MaxMana);
        }

        public int getBonusMana()
        {
            return getEquipmentStat(e => e.BonusMana);
        }
    }
}
