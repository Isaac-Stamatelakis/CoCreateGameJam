using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
    public class GrabbedItem : MonoBehaviour
    {
        private static GrabbedItem instance;
        public void Awake() {
            instance = this;
        }
        private IItem item;

        public static GrabbedItem Instance { get => instance; }

        public void setItem(IItem item) {
            this.item = item;
        }
    }

}
