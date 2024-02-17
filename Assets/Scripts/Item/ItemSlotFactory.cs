using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryModule {
    public static class ItemSlotFactory 
    {
        public static GameObject create(string id) {
            Equipment equipment = EquipmentRegistry.getInstance().getEquipment(id);
            if (equipment == null) {
                
            }
            return null;
        }
    }
}

