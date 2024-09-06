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
using UI;

namespace Creatures.UI {
    public abstract class CreatureDetailedDisplay : UIInventoryDisplayer<EquipedCreature>
    {
        [SerializeField] private bool recruitable;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image creatureImage;
        [SerializeField] private TMP_InputField nicknameField;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Slider xpSlider;
        [SerializeField] protected DetailedCreatureEquipmentListUI itemInventory;
        [SerializeField] private Button recruitButton;
        [SerializeField] private Button infoButton;
        List<InventoryUI<EnchantedEquipment>> syncedEquipmentLists = new List<InventoryUI<EnchantedEquipment>>();
        protected EquipedCreature equipedCreature;
        public EquipedCreature EquipedCreature { get => equipedCreature;}
        public List<InventoryUI<EnchantedEquipment>> SyncedEquipmentLists { get => syncedEquipmentLists; }
        public bool Recruitable { get => recruitable; set => recruitable = value; }

        public void Start() {
            nicknameField.onValueChanged.AddListener((string value) => {
                if (equipedCreature != null) {
                    equipedCreature.Nickname = value;
                }
            });
            recruitButton.onClick.AddListener(() => {
                if (recruitable) {
                    
                } else {
                    PlayerIO.Instance.removeCreatureFromTeam(equipedCreature);
                    onDismiss();
                }
            });
            addListeners();
        }

        protected abstract void addListeners();
        protected abstract void onDismiss();

        public override void display(EquipedCreature element)
        {
            this.equipedCreature = element;
            nameText.text = element.Creeture.name;
            creatureImage.sprite = element.Creeture.Sprite;
            nicknameField.text = element.Nickname;
            
            levelText.text = element.Level.ToString();
            float experienceToLevel = CreatureUtils.getExperienceToLevel(element.Level);
            xpSlider.value = element.XP/experienceToLevel;
            displayRecruitButton();
            

            GlobalUtils.clamp<EnchantedEquipment>(element.EnchantedEquipment,Global.MAX_CREATURE_EQUIPMENT);
            itemInventory.display(element.EnchantedEquipment);
        }

        public void addSyncedEquipmentList(InventoryUI<EnchantedEquipment> equipmentListUI) {
            syncedEquipmentLists.Add(equipmentListUI);
        }

        public void setRecruitable(bool recruitable) {
            this.recruitable = recruitable;
            displayRecruitButton();
        }

        private void displayRecruitButton() {
            TextMeshProUGUI recruitButtonText = recruitButton.GetComponentInChildren<TextMeshProUGUI>();
            recruitButtonText.text = recruitable ? "Recruit" : "Dismiss";
        }
    }
}

