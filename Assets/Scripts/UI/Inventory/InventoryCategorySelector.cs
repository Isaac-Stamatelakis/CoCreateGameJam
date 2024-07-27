using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Items;
namespace UI.Inventory {
    public class InventoryCategorySelector : MonoBehaviour
    {
        [SerializeField] private Button creatureButton;
        [SerializeField] private Button equipmentButton;
        [SerializeField] private Button lootboxButton;
        [SerializeField] private Button currencyButton;
        [SerializeField] private InventoryController controller;

        public void Start() {
            creatureButton.onClick.AddListener(() => {
                controller.setCategory(InventoryUIMode.Creature);
            });
            equipmentButton.onClick.AddListener(() => {
                controller.setCategory(InventoryUIMode.Equipment);
            });
            lootboxButton.onClick.AddListener(() => {
                controller.setCategory(InventoryUIMode.Lootbox);
            });
            currencyButton.onClick.AddListener(() => {
                controller.setCategory(InventoryUIMode.Currency);
            });
        }
        public void highlightCategory(InventoryUIMode mode) {
            switch (mode) {
                case InventoryUIMode.Creature:
                    break;
                case InventoryUIMode.Equipment:
                    break;
                case InventoryUIMode.Lootbox:
                    break;
                case InventoryUIMode.Currency:
                    break;
            }
        }
    }
}

