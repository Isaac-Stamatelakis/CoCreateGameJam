using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Inventory {
    public abstract class MainInventoryUI<T> : InventoryUI<T>, IDetailedViewDisplayer where T : IDisplayable
    {
        [SerializeField] private InventoryController controller;
        [SerializeField] private UIDisplayer<T> detailedDisplayPrefab;

        public void displayDetailedView(int index)
        {
            UIDisplayer<T> detailedDisplayer = GameObject.Instantiate(detailedDisplayPrefab);
            detailedDisplayer.display(elements[index],this,index);
            detailedDisplayer.name = $"{typeof(T)}{index} detailed Display";
            controller.setDetailedDisplay(detailedDisplayer.gameObject);
        }
    }
}

