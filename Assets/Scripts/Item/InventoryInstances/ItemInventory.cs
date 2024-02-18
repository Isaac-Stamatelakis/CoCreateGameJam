using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryModule {
    public class ItemInventory : Inventory<Equipment>
    {
        protected override void loadSlot(int index, int row, int col)
        {
            GameObject slot;
            if (index < elements.Count) {
                slot = ItemSlotFactory.fromEquipment(elements[index]);
            } else {
                slot = ItemSlotFactory.fromEquipment(null);
            }
            slot.transform.SetParent(transform,false);
            slot.name = "slot[" + row + "," + col + "]";
        }
    }
}

