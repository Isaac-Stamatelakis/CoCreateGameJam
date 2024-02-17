using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace InventoryModule {
    public class ItemInventory : MonoBehaviour
    {
        private List<string> strings;
        private PlayerIO playerIO;
        // Start is called before the first frame update
        void Start()
        {
            playerIO = GameObject.Find("Player").GetComponent<PlayerIO>();
            loadSlots();

        }

        private void loadSlots() {
            GameObject prefab = Resources.Load<GameObject>("Inventory/Panel");
            List<string> ids = playerIO.EquipmentIds;
            int columns = ids.Count/7+1;
            for (int col = 0; col < columns; col++) {
                for (int row = 0; row < 7; row ++) {
                    int index = col*7+row;
                    GameObject slot;
                    if (index < ids.Count) {
                        slot = ItemSlotFactory.create(ids[index]);
                    } else {
                        slot = ItemSlotFactory.create(null);
                    }
                    
                    slot.transform.SetParent(transform,false);
                    slot.name = "slot[" + row + "," + col + "]";
                }
            }
        }
    }
}

