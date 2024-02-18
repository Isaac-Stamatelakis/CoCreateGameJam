using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreatureModule;

namespace InventoryModule {
    public class CreatureInventory : Inventory<Creeture>
    {

        protected override void loadSlot(int index, int row, int col)
        {
            GameObject slot;
            if (index < elements.Count) {
                slot = ItemSlotFactory.fromCreature(elements[index]);
            } else {
                slot = ItemSlotFactory.fromCreature(null);
            }
            slot.transform.SetParent(transform,false);
            slot.name = "slot[" + row + "," + col + "]";
        }
    }
}

