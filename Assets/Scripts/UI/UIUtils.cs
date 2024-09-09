using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
    public enum UITransition {
        Left,
        Right,
    }
    public static class UIUtils
    {
        public static T instantiateUIPrefab<T>(T prefab, Transform parent, UITransition? uITransition = null) where T : MonoBehaviour {
            T instantiated = GameObject.Instantiate(prefab);
            if (parent != null) {
                instantiated.transform.SetParent(parent,false);
            }
            if (uITransition == null) {
                return instantiated;
            }
            switch (uITransition) {
                case UITransition.Left:
                    break;
                case UITransition.Right:
                    break;
            }
            return instantiated;
        }
    }
}

