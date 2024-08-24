using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.Equipment {
    public class EquipmentFactory : SerializationFactory<Equipment, SerializedEquipment>
    {
        protected override Equipment deserializeValue(SerializedEquipment sValue)
        {
            return LootableRegistry.getInstance().getLootable<Equipment>(sValue.id);
        }

        protected override SerializedEquipment formatValue(Equipment value)
        {
            return new SerializedEquipment(value.getId());
        }
    }

    public class SerializedEquipment {
        public string id;

        public SerializedEquipment(string id)
        {
            this.id = id;
        }
    }
}

