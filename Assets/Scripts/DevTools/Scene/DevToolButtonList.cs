using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;
using UnityEngine.UI;

namespace DevTools {
    public class DevToolButtonList : MonoBehaviour
    {
        [SerializeField] private BackButtonUI backButton;
        [SerializeField] private List<SerializableKVP<Button,GameObject>> buttonPrefabs;
        
        public void Start() {
            for (int i = 0; i < buttonPrefabs.Count; i++) {
                var kvp = buttonPrefabs[i];
                Button button = kvp.key;
                GameObject prefab = kvp.value;
                if (button == null && prefab == null) {
                    Debug.LogWarning($"Button, prefab entry {i} are both null");
                }
                if (button == null) {
                    Debug.LogWarning($"Button entry {i} is null");
                }
                if (prefab == null) {
                    Debug.LogWarning($"Prefab entry {i} is null");
                }
                if (button == null || prefab == null) {
                    continue;
                }
                button.onClick.AddListener(() => {
                    GameObject instantiated = GameObject.Instantiate(prefab);
                    backButton.parentObject = instantiated;
                    backButton.gameObject.SetActive(true);
                });
            }
        }
    }

    


}

