using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UI.Lists {
    public interface IUIDisplayer<T> where T : IDisplayable
    {
        public void display(T element, int index, InventoryUI<T> inventory);
    }
    public interface IPanelDisplay {
        public void setPanelColor(Color color);
        public Color getColor();
    }
}

