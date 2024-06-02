using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;

namespace LootBoxes {
    public class LootBoxButtonMain : MonoBehaviour
    {
        [SerializeField] public Button button;
        [SerializeField] public LootboxUIController lootboxUIController;
        [SerializeField] public PlayerIO playerIO;
        // Start is called before the first frame update
        void Start()
        {
            button.onClick.AddListener(navigate);
        }

        void OnDestroy() {
            button.onClick.RemoveAllListeners();
        }

        private void navigate() {
            lootboxUIController.gameObject.SetActive(true);
            lootboxUIController.init(playerIO.LootBoxes);
            gameObject.SetActive(false);
        }
    }
}

