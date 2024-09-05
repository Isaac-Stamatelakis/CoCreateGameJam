using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
    public class UIController : MonoBehaviour
    {
        private static UIController instance;
        public static UIController Instance {get => instance;}
        public GameObject popup;
        public GameObject label;
        public void Awake() {
            instance = this;
        }
        public void displayElement(Transform element) {
            GlobalUtils.deleteChildren(transform);
            element.SetParent(transform,false);
        }

        public void displayNested(Transform element) {
            element.transform.SetParent(transform,false);
        }

        public void displayPopUp(GameObject newPopup, GameObject objectToPlaceAbove) {
            if (popup != null) {
                GameObject.Destroy(popup);
            }
            newPopup.transform.SetParent(transform,false);
            RectTransform rectTransform = (RectTransform) objectToPlaceAbove.transform;
            Vector3 position = new Vector3(
                rectTransform.position.x, 
                rectTransform.position.y + rectTransform.rect.height/2, 
                rectTransform.position.z
            );
            newPopup.transform.position = position;
        }

        
    }
}

