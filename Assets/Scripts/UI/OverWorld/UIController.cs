using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
    public class UIController : MonoBehaviour
    {
        public void displayElement(Transform element) {
            GlobalUtils.deleteChildren(transform);
            element.SetParent(transform,false);
        }

        public void displayNested(Transform element) {
            element.transform.SetParent(transform,false);
        }
    }
}

