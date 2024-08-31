using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.Equipment {
    public class EquipmentFactory : SerializationFactory<EnchantedEquipment, SerializedEquipment>
    {
        protected override EnchantedEquipment deserializeValue(SerializedEquipment sValue)
        {
            return new EnchantedEquipment(
                LootableRegistry.getInstance().getLootable<Equipment>(sValue.id),
                EnchantmentRegistry.getEnchantment(sValue.enchantmentId)
            );
        }

        protected override SerializedEquipment formatValue(EnchantedEquipment value)
        {
            return new SerializedEquipment(
                value.getId(),
                value.enchant.Id
            );
        }
    }

    public class SerializedEquipment {
        public string id;
        public string enchantmentId;

        public SerializedEquipment(string id, string enchantmentId)
        {
            this.id = id;
            this.enchantmentId = enchantmentId;
        }
    }
}

