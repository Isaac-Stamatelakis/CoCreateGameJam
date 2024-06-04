using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
    public class UIController : MonoBehaviour
    {
        public void displayElement(Transform element) {
            for (int i = 0; i < transform.childCount; i++) {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
            element.SetParent(transform,false);
        }
    }
}

