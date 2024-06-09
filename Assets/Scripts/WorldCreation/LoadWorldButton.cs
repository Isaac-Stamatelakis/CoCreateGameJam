using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace WorldCreationModule {
    public class LoadWorldButton : MonoBehaviour
    {
        [SerializeField] public Button button;
        [SerializeField] public int index;
        // Start is called before the first frame update
        void Start()
        {
            button.onClick.AddListener(loadWorld);
        }
        private void loadWorld() {
            string worldName = WorldCreation.getWorldName(index);
            Debug.Log(worldName);
            Debug.Log(Application.persistentDataPath);
            if (!WorldCreation.worldExists(worldName)) {
                Debug.Log(Application.persistentDataPath);
                WorldCreation.createWorld(worldName);
            }
            Global.WorldName = worldName;
            SceneManager.LoadScene("SelectorScene");
        }
    }
}

