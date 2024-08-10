using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory {
    public abstract class UIInventoryDisplayer<T> : MonoBehaviour, IUIDisplayer<T>, IPointerClickHandler where T : IDisplayable
    {
        protected InventoryUI<T> inventory;
        protected int index;
        public abstract void display(T element);
        public void display(T element, int index, InventoryUI<T> inventory) {
            this.index = index;
            this.inventory = inventory;
            display(element);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left) {
                inventory.leftClick(index);
            } else if (eventData.button == PointerEventData.InputButton.Right) {
                inventory.rightClick(index);
            }
        }
    }
}

