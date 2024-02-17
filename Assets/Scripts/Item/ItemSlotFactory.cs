using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryModule {
    public static class ItemSlotFactory 
    {
        public static GameObject create(string id) {
            Equipment equipment = EquipmentRegistry.getInstance().getEquipment(id);
            GameObject prefab = GameObject.Instantiate(Resources.Load<GameObject>("Inventory/Panel"));
            if (equipment != null) {
                Image image = prefab.transform.Find("Image").GetComponent<Image>();
                image.sprite  = equipment.sprite;
            }
            return prefab;
        }
    }
}

