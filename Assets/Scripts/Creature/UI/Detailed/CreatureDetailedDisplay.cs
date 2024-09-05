using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Creatures;
using Items;
using UI.Lists;
using Player;
using Items.Equipment;

namespace Creatures.UI {
    public class CreatureDetailedDisplay : UIInventoryDisplayer<EquipedCreature>
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image creatureImage;
        [SerializeField] private TMP_InputField nicknameField;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Slider xpSlider;
        [SerializeField] private DetailedCreatureEquipmentListUI itemInventory;
        [SerializeField] private Button recruitButton;
        [SerializeField] private Button equipButton;
        [SerializeField] private Button infoButton;
        List<InventoryUI<EnchantedEquipment>> syncedEquipmentLists = new List<InventoryUI<EnchantedEquipment>>();
        private EquipedCreature equipedCreature;
        public EquipedCreature EquipedCreature { get => equipedCreature;}
        public List<InventoryUI<EnchantedEquipment>> SyncedEquipmentLists { get => syncedEquipmentLists; }

        public void Start() {
            nicknameField.onValueChanged.AddListener((string value) => {
                if (equipedCreature != null) {
                    equipedCreature.Nickname = value;
                }
            });
        }

        public override void display(EquipedCreature element)
        {
            this.equipedCreature = element;
            nameText.text = element.Creeture.name;
            creatureImage.sprite = element.Creeture.Sprite;
            nicknameField.text = element.Nickname;
            
            levelText.text = element.Level.ToString();
            float experienceToLevel = CreatureUtils.getExperienceToLevel(element.Level);
            xpSlider.value = element.XP/experienceToLevel;

            PlayerIOUtils.clamp<EnchantedEquipment>(element.EnchantedEquipment,Global.MAX_CREATURE_EQUIPMENT);
            itemInventory.display(element.EnchantedEquipment);
        }

        public void addSyncedEquipmentList(InventoryUI<EnchantedEquipment> equipmentListUI) {
            syncedEquipmentLists.Add(equipmentListUI);
        }
    }
}

