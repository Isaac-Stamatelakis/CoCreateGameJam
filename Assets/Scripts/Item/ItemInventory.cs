using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryModule {
    public class ItemInventory : MonoBehaviour
    {
        public int columns;
        private List<string> strings;
        // Start is called before the first frame update
        void Start()
        {
            loadSlots();
        }

        private void loadSlots() {
            GameObject prefab = Resources.Load<GameObject>("Inventory/Panel");
            for (int col = 0; col < columns; col++) {
                for (int row = 0; row < 7; row ++) {
                    GameObject slot = GameObject.Instantiate(prefab);
                    slot.transform.SetParent(transform,false);
                    slot.name = "slot[" + row + "," + col + "]";
                }
            }
        }
    }
}

