using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Lists;
using TMPro;
using UI;
using UnityEngine.UI;

namespace Items.Equipment.UI {
    public class EquipmentListSorterUI : InventorySorter<EnchantedEquipment>
    {
        [SerializeField] private TMP_Dropdown rarityDropdown;
        [SerializeField] private TMP_Dropdown enchantmentDropdown;
        [SerializeField] private Button enableAllStats;
        [SerializeField] private Button disableAllStats;
        [SerializeField] private ImageToggleButtonUI healthToggle;
        [SerializeField] private ImageToggleButtonUI speedToggle;
        [SerializeField] private ImageToggleButtonUI abilityToggle;
        [SerializeField] private ImageToggleButtonUI strengthToggle;
        [SerializeField] private ImageToggleButtonUI maxManaToggle;
        [SerializeField] private ImageToggleButtonUI armorToggle;
        [SerializeField] private ImageToggleButtonUI bonusManaToggle;
        private List<Rarity?> rarities;
        private int rarityIndex;
        HashSet<string> viewedStats = new HashSet<string>();
        Dictionary<string,StatGetter> statGetterDict = new Dictionary<string, StatGetter>();
        List<ImageToggleButtonUI> statToggleButtons = new List<ImageToggleButtonUI>();
        protected override void addSortingFeatures()
        {
            Rarity[] nonNullRarities = GlobalUtils.getAllEnums<Rarity>();
            rarities = new List<Rarity?>{null};
            rarityDropdown.options = new List<TMP_Dropdown.OptionData>{
                new TMP_Dropdown.OptionData("All Rarities")
            };
            foreach (Rarity rarity in nonNullRarities) {
                rarityDropdown.options.Add(new TMP_Dropdown.OptionData(rarities.ToString()));
            }
            rarityDropdown.value=0;
            rarityDropdown.onValueChanged.AddListener((int index) => {
                this.rarityIndex = index;
            });
            enchantmentDropdown.options = new List<TMP_Dropdown.OptionData>{
                new TMP_Dropdown.OptionData("All Enchantments")
            };
            addToggleButton(healthToggle, "health", (EnchantedEquipment equipment) => equipment.getHealth());
            addToggleButton(strengthToggle, "strength", (EnchantedEquipment equipment) => equipment.getAttack());
            addToggleButton(armorToggle, "armor", (EnchantedEquipment equipment) => equipment.getDefense());
            addToggleButton(abilityToggle, "ability", (EnchantedEquipment equipment) => equipment.getAbility());
            addToggleButton(speedToggle, "speed", (EnchantedEquipment equipment) => equipment.getSpeed());
            addToggleButton(maxManaToggle, "maxMana", (EnchantedEquipment equipment) => equipment.getMaxMana());
            addToggleButton(bonusManaToggle, "bonusMana", (EnchantedEquipment equipment) => equipment.getBonusMana());

            disableAllStats.onClick.AddListener(() => {
                viewedStats = new HashSet<string>();
                foreach (ImageToggleButtonUI toggleButtonUI in statToggleButtons) {
                    toggleButtonUI.setActiveNoCallBack(false);
                }
                display();
            });

            enableAllStats.onClick.AddListener(() => {
                foreach (string key in statGetterDict.Keys) {
                    viewedStats.Add(key);
                }
                foreach (ImageToggleButtonUI toggleButtonUI in statToggleButtons) {
                    toggleButtonUI.setActiveNoCallBack(true);
                }
                display();
            });
            
            
        }

        protected override void applySorting()
        {
            List<EnchantedEquipment> displayed = InventorySortingUtils.sortEnum<EnchantedEquipment,Rarity>(elements,rarities[rarityIndex],rarityGetter);
            //displayStats();
        }

        private void displayStats() {
            foreach (string stat in viewedStats) {
                StatGetter getter = statGetterDict[stat];
                List<EnchantedEquipment> passed = new List<EnchantedEquipment>();
                foreach (EnchantedEquipment enchantedEquipment in displayed) {
                    if (getter(enchantedEquipment) > 0) {
                        passed.Add(enchantedEquipment);
                    }
                }
                displayed = passed;
            }
        }

        private void addToggleButton(ImageToggleButtonUI toggleButton, string key, StatGetter getter) {
            statToggleButtons.Add(toggleButton);
            viewedStats.Add(key);
            statGetterDict[key] = getter;
            toggleButton.setCallBack((bool active) => toggleStat(active,key));
        }
        private void toggleStat(bool active, string key) {
            if (active) {
                viewedStats.Add(key);
            } else {
                viewedStats.Remove(key);
            }
            display();
        }
        private delegate float StatGetter(EnchantedEquipment enchant);

        private float getHealth(EnchantedEquipment enchantedEquipment) => enchantedEquipment.Equipment.Health;

        private InventorySortingUtils.EnumGetter<EnchantedEquipment, Rarity> rarityGetter = enchantedEquipment => enchantedEquipment.Equipment.Rarity;
    }
}

