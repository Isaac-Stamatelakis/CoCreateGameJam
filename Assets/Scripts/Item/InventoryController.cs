using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Player;

namespace InventoryModule {
    public class InventoryController : MonoBehaviour
    {
        
        public Button mButton;
        private TextMeshProUGUI mButtonText;
        private Mode mode;
        private GameObject currentInventory;
        private GameObject itemInventoryPrefab;
        private GameObject creatureInventoryPrefab;
        private PlayerIO playerIO;
        // Start is called before the first frame update
        void Start()
        {
            mButton.onClick.AddListener(onButtonClick);
            mButtonText = mButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            itemInventoryPrefab = Resources.Load<GameObject>("Inventory/ItemInventory");
            creatureInventoryPrefab = Resources.Load<GameObject>("Inventory/CreatureInventory");
            playerIO = GameObject.Find("Player").GetComponent<PlayerIO>();
            setState();
        }

        // Update is called once per frame
        void onButtonClick()
        {
            switch (mode) {
                case Mode.Creature:
                    mode = Mode.Equipment;
                    break;
                case Mode.Equipment:
                    mode = Mode.Creature;
                    break;
            }
            setState();
        }

        private void setState() {
            mButtonText.text = mode.ToString();
            if (currentInventory != null) {
                GameObject.Destroy(currentInventory);
            }
            switch (mode) {
                case Mode.Equipment:
                    currentInventory = GameObject.Instantiate(itemInventoryPrefab);
                    ItemInventory itemInventory = currentInventory.GetComponentInChildren<ItemInventory>();
                    itemInventory.initalize(playerIO.GetEquipment());
                    break;
                case Mode.Creature:
                    currentInventory = GameObject.Instantiate(creatureInventoryPrefab);
                    CreatureInventory creatureInventory = currentInventory.GetComponentInChildren<CreatureInventory>();
                    creatureInventory.initalize(playerIO.GetCreetures());
                    break;
            }
            currentInventory.transform.SetParent(transform,false);
        }

        private enum Mode {
            Equipment,
            Creature
        }
    }
}
