using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Creatures;
using Items;

namespace UI.Inventory {
    public class CreatureDetailedDisplay : UIDisplayer<EquipedCreeture>
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image creatureImage;
        [SerializeField] private TMP_InputField nicknameField;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Slider xpSlider;
        [SerializeField] private ItemInventoryUI itemInventory;
        [SerializeField] private Button recruitButton;
        [SerializeField] private Button equipButton;
        [SerializeField] private Button infoButton;

        public override void display(EquipedCreeture element, InventoryUI<EquipedCreeture> inventory, int index)
        {
            nameText.text = element.Creeture.name;
            creatureImage.sprite = element.Creeture.Sprite;
            nicknameField.text = element.Nickname;
            nicknameField.onValueChanged.AddListener((string value) => {
                element.Nickname = value;
            });
            levelText.text = element.Level.ToString();
            float experienceToLevel = CreatureUtils.getExperienceToLevel(element.Level);
            xpSlider.value = element.XP/experienceToLevel;

            int i = 0;
            List<Equipment> toDisplay = new List<Equipment>();
            while (i < element.Equipment.Count && i < 3) {
                toDisplay.Add(element.Equipment[i]);
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

