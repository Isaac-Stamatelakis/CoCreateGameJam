using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Player;
using Creatures;
using Items;

namespace UI.Inventory {
    public interface IDetailedViewDisplayer {
        public void displayDetailedView(int index);
    }
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private GridLayoutGroup inventoryContainer;
        [SerializeField] private MainCreatureInventoryUI creatureInventory;
        [SerializeField] private MainItemInventoryUI equipmentInventory;
        [SerializeField] private MainLootboxInventory lootboxInventory;
        [SerializeField] private InventoryCategorySelector inventoryCategorySelector;
        [SerializeField] private Transform detailedDisplayContainer;
        private InventoryUIMode mode;
        public Transform DetailedDisplayContainer { get => detailedDisplayContainer;}

        // Start is called before the first frame update
        void Start()
        {
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
                    List<EquipedCreeture> temp = new List<EquipedCreeture>{
                        new EquipedCreeture(
                            CreatureRegistry.getInstance().getCreature("goof_ball"),
                            new List<Items.Equipment>()
                        ),
                        new EquipedCreeture(
                            CreatureRegistry.getInstance().getCreature("mike"),
                            new List<Items.Equipment>()
                        ),
                        new EquipedCreeture(
                            CreatureRegistry.getInstance().getCreature("dragoon"),
                            new List<Items.Equipment>()
                        ),
                        new EquipedCreeture(
                            CreatureRegistry.getInstance().getCreature("crog"),
                            new List<Items.Equipment>()
                        ),
                        new EquipedCreeture(
                            CreatureRegistry.getInstance().getCreature("omongus"),
                            new List<Items.Equipment>()
                        ),
                    };
                    creatureInventory.display(PlayerIO.Instance.EquipedCreetures);
                    titleText.text = "Creetures";
                    //creatureInventory.display(temp);
                    break;
                case InventoryUIMode.Equipment:
                    List<Equipment> tempEquipment = new List<Equipment>{
                        EquipmentRegistry.getInstance().getEquipment("mazda5")
                    };
                    titleText.text = "Equipment";
                    equipmentInventory.display(tempEquipment);
                    break;
                case InventoryUIMode.Lootbox:
                    lootboxInventory.display(PlayerIO.Instance.LootBoxes);
                    titleText.text = "LootBoxes";
                    break;
                case InventoryUIMode.Currency:
                    titleText.text = "LootBoxes";
                    break;
            }
        }

        public void setDetailedDisplay(GameObject detailedDisplay) {
            for (int i = 0; i < detailedDisplayContainer.childCount; i++) {
                GameObject.Destroy(detailedDisplayContainer.GetChild(i).gameObject);
            }
            detailedDisplay.transform.SetParent(detailedDisplayContainer,false);
        }
    }
    public enum InventoryUIMode {
            Equipment,
            Creature,
            Lootbox,
            Currency
        }
}
