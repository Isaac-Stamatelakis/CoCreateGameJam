using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Player;
using Creatures;
using Items;
using System.Linq;
using Items.Equipment;

namespace UI.Inventory {
    public class InventoryController : MonoBehaviour, IDisplayController
    {
        [SerializeField] private Button backButton;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private GridLayoutGroup inventoryContainer;
        private CreatureMainInventoryUI creatureInventory;
        private EquipmentMainInventoryUI equipmentInventory;
        private MainLootboxInventory lootboxInventory;
        private CraftingItemMainInventoryUI craftingInventory;
        [SerializeField] private InventoryCategorySelector inventoryCategorySelector;
        [SerializeField] private Transform detailedDisplayContainer;
        private InventoryUIMode mode;
        public Transform DetailedDisplayContainer { get => detailedDisplayContainer;}

        // Start is called before the first frame update
        void Start()
        {
            creatureInventory = inventoryContainer.GetComponent<CreatureMainInventoryUI>();
            equipmentInventory = inventoryContainer.GetComponent<EquipmentMainInventoryUI>();
            lootboxInventory = inventoryContainer.GetComponent<MainLootboxInventory>();
            craftingInventory = inventoryContainer.GetComponent<CraftingItemMainInventoryUI>();
            setCategory(InventoryUIMode.Creature);
            backButton.onClick.AddListener(() => {
                gameObject.SetActive(false);
            });
            
        }

        public void setCategory(InventoryUIMode newMode) {
            if (newMode == mode) {
                return;
            }
            for (int i = 0; i < inventoryContainer.transform.childCount; i++) {
                GameObject.Destroy(inventoryContainer.transform.GetChild(i).gameObject);
            }
            mode = newMode;
            List<IDisplayable> displayables = new List<IDisplayable>();
            switch (newMode) {
                case InventoryUIMode.Creature:
                    creatureInventory.display(PlayerIO.Instance.EquipedCreetures);
                    titleText.text = "Creetures";
                    break;
                case InventoryUIMode.Equipment:
                    List<Equipment> tempEquipment = new List<Equipment>{
                        LootableRegistry.getInstance().getLootable<Equipment>("mazda5")
                    };
                    titleText.text = "Equipment";
                    equipmentInventory.display(tempEquipment);
                    break;
                case InventoryUIMode.Lootbox:
                    lootboxInventory.display(PlayerIO.Instance.LootBoxes.Cast<StackableItemSlot>().ToList());
                    titleText.text = "Loot Boxes";
                    break;
                case InventoryUIMode.Currency:
                    titleText.text = "Currencies";
                    break;
                case InventoryUIMode.CraftingItem:
                    titleText.text = "Crafting Items";
                    craftingInventory.display(PlayerIO.Instance.CraftingItems.Cast<StackableItemSlot>().ToList());
                    break;
                case InventoryUIMode.Consumables:
                    titleText.text = "Consumables";
                    break;

            }
        }

        public Transform getDetailedDisplayContainer()
        {
            return detailedDisplayContainer;
        }
    }
    public enum InventoryUIMode {
            Equipment,
            Creature,
            Lootbox,
            Currency,
            CraftingItem,
            Consumables
        }
}
