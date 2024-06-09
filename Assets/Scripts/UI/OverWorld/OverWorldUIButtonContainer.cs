using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.OverWorld {
    public class OverWorldUIButtonContainer : MonoBehaviour
    {
        [SerializeField] private UIController uIController;
        [SerializeField] private List<ButtonObjectPair> prefabButtons;
        [SerializeField] private List<ButtonObjectPair> activateButtons;

        public void Start() {
            foreach (ButtonObjectPair buttonPrefabPair in prefabButtons) {
                buttonPrefabPair.button.onClick.AddListener(() => {
                    GameObject clone = GameObject.Instantiate(buttonPrefabPair.gameObject);
                    uIController.displayElement(clone.transform);
                });
            }
            foreach (ButtonObjectPair buttonObjectPair in activateButtons) {
                buttonObjectPair.button.onClick.AddListener(() => {
                    buttonObjectPair.gameObject.SetActive(true);
                });
            }
        }

        [System.Serializable]
        private class ButtonObjectPair {
            public Button button;
            public GameObject gameObject;
        }
    }
    
}

