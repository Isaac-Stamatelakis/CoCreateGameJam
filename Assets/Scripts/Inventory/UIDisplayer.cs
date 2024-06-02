using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Inventory {
    public abstract class UIDisplayer<T> : MonoBehaviour, IUIDisplayer<T> where T : IDisplayable
    {
        public abstract void display(T element, InventoryUI<T> inventory, int index);
    }
}

