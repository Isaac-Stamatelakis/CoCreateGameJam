using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class BackButtonUI : MonoBehaviour
    {
        [SerializeField] private bool destroyOnClick = true;
        [SerializeField] public GameObject parentObject;
        public void Start() {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(() => {
                if (destroyOnClick) {
                    GameObject.Destroy(parentObject);
                } else {
                    parentObject.gameObject.SetActive(false);
                }
            });
        }
    }
}

