using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using UnityEngine.EventSystems;

namespace InventoryModule {
    public abstract class Inventory<T> : MonoBehaviour
    {
        public int rowSize = 6;
        protected PlayerIO playerIO;
        protected GameObject slotPrefab;
        protected GrabbedItem grabbedItem;
        protected List<T> elements;
        // Start is called before the first frame update
        void Start()
        {
            slotPrefab = Resources.Load<GameObject>("Inventory/Panel");
            grabbedItem = GameObject.Find("GrabbedItem").GetComponent<GrabbedItem>();
        }
        public void initalize(List<T> elements) {
            this.elements = elements;
            loadSlots();
        }
        protected void loadSlots() {
            int columns = getColumns();
            for (int col = 0; col < columns; col++) {
                for (int row = 0; row < rowSize; row ++) {
                    int index = col*rowSize+row;
                    loadSlot(index, row,col);
                }
            }
        }

        protected abstract void loadSlot(int index, int row, int col);
        protected int getColumns() {
            return elements.Count/rowSize+1;
        }
    }
}

