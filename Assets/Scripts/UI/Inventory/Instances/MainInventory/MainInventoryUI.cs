using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Inventory {

    public interface IDisplayController {
        public Transform getDetailedDisplayContainer();
        public void setDetailedDisplay(GameObject detailedDisplay) {
            Transform detailedDisplayContainer = getDetailedDisplayContainer();
            GlobalUtils.deleteChildren(detailedDisplayContainer);
            detailedDisplay.transform.SetParent(detailedDisplayContainer,false);
        }
    }
    public abstract class MainInventoryUI<T> : InventoryUI<T> where T : IDisplayable
    {
        private IDisplayController controller;
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
                controller = parentTransform.GetComponent<IDisplayController>();
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

