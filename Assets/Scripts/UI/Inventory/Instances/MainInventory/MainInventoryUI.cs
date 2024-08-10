using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Inventory {
    public abstract class MainInventoryUI<T> : InventoryUI<T> where T : IDisplayable
    {
        private InventoryController controller;
        [SerializeField] private UIInventoryDisplayer<T> detailedDisplayPrefab;
        public void Start() {
            getController();
        }
        public override void leftClick(int index)
        {
            UIInventoryDisplayer<T> detailedDisplayer = GameObject.Instantiate(detailedDisplayPrefab);
            detailedDisplayer.display(elements[index],index,this);
            detailedDisplayer.name = $"{typeof(T)}{index} detailed Display";
            controller.setDetailedDisplay(detailedDisplayer.gameObject);
        }

        private void getController() {
            Transform parentTransform = transform.parent;
            while (parentTransform != null) {
                controller = parentTransform.GetComponent<InventoryController>();
                if (controller != null) {
                    return;
                }
                parentTransform = parentTransform.parent;
            }
        }
        public override void rightClick(int index)
        {
            
        }
    }
}

