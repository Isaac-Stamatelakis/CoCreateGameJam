using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Inventory {
    public abstract class UIDisplayer<T> : MonoBehaviour where T : IDisplayable {
        public abstract void display(T element);
    }
    public abstract class UIInventoryDisplayer<T> : MonoBehaviour, IUIDisplayer<T> where T : IDisplayable
    {
        public abstract void display(T element, InventoryUI<T> inventory, int index);
    }

    public abstract class ClickableUIDisplayer<T> : UIInventoryDisplayer<T>, IPointerClickHandler where T : IDisplayable
    {
        public void OnPointerClick(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                leftClick();
            } else if (eventData.button == PointerEventData.InputButton.Right) {
                rightClick();
            }
        }
        public abstract void rightClick();
        public abstract void leftClick();
    }
}

