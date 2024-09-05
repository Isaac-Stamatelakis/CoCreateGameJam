using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Creatures;
using Items;
using UI.Lists;
using Items.Equipment;

namespace Creatures.UI {
    public class CreatureDetailedDisplay : UIInventoryDisplayer<EquipedCreature>
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image creatureImage;
        [SerializeField] private TMP_InputField nicknameField;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Slider xpSlider;
        [SerializeField] private StaticEquipmentInventoryUI itemInventory;
        [SerializeField] private Button recruitButton;
        [SerializeField] private Button equipButton;
        [SerializeField] private Button infoButton;

        private EquipedCreature equipedCreature;
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

            int i = 0;
            List<EnchantedEquipment> toDisplay = new List<EnchantedEquipment>();
            while (i < element.EnchantedEquipment.Count && i < 3) {
                toDisplay.Add(element.EnchantedEquipment[i]);
                i++;
            }
            while (i < 3) {
                toDisplay.Add(null);
                i++;
            }
            itemInventory.display(toDisplay);
        }
    }
}

