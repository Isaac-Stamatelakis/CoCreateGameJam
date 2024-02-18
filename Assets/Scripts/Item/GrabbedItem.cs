using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryModule {
    public class GrabbedItem : MonoBehaviour
    {
        private IItem item;
        public void setItem(IItem item) {
            this.item = item;
            GameObject panel = ItemSlotFactory.fromItem(item);
        }
    }

}
