using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
    public class ItemSlotSerializationFactory : SerializationFactory<ItemSlot, SerializedItemSlot>
    {
        protected override ItemSlot deserializeValue(SerializedItemSlot sValue)
        {
            return new ItemSlot(
                LootableRegistry.getInstance().getLootable(sValue.id),
                sValue.amount
            );
        }

        protected override SerializedItemSlot formatValue(ItemSlot value)
        {
            if (value == null || value.Lootable == null) {
                return null;
            }
            return new SerializedItemSlot(
                value.Lootable.getId(),
                value.Amount
            );
        }
    }

    public class SerializedItemSlot {
        public string id;
        public int amount;

        public SerializedItemSlot(string id, int amount)
        {
            this.id = id;
            this.amount = amount;
        }
    }
}


