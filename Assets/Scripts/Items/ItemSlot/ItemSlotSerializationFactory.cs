using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
    public class IntItemSlotSerializationFactory : SerializationFactory<IntItemSlot, SerializedItemSlot>
    {
        protected override IntItemSlot deserializeValue(SerializedItemSlot sValue)
        {
            return new IntItemSlot(
                LootableRegistry.getInstance().getLootable(sValue.id),
                sValue.amount
            );
        }

        protected override SerializedItemSlot formatValue(IntItemSlot value)
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

    public class DoubleItemSlotSerializationFactory : SerializationFactory<DoubleItemSlot, SerializedDoubleItemSlot>
    {
        protected override DoubleItemSlot deserializeValue(SerializedDoubleItemSlot sValue)
        {
            return new DoubleItemSlot(
                LootableRegistry.getInstance().getLootable(sValue.id),
                sValue.amount
            );
        }

        protected override SerializedDoubleItemSlot formatValue(DoubleItemSlot value)
        {
            if (value == null || value.Lootable == null) {
                return null;
            }
            return new SerializedDoubleItemSlot(
                value.Lootable.getId(),
                value.Amount
            );
        }
    }

    public class SerializedDoubleItemSlot {
        public string id;
        public double amount;

        public SerializedDoubleItemSlot(string id, double amount)
        {
            this.id = id;
            this.amount = amount;
        }
    }
}


