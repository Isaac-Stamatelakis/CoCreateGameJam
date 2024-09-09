using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Lists;
using Creatures;
using UnityEngine.UI;
using TMPro;
using Items.Equipment;

namespace UI.TeamSelect {
    public class TeamSelectCreatureUI : UIInventoryDisplayer<EquipedCreature>
    {
        [SerializeField] private Image creatureImage;
        [SerializeField] private TextMeshProUGUI creatureName;
        [SerializeField] private TextMeshProUGUI nickName;
        [SerializeField] private TextMeshProUGUI rarity;
        [SerializeField] private TextMeshProUGUI level;
        [SerializeField] private InventoryUI<EnchantedEquipment> equipment;
        [SerializeField] private TextMeshProUGUI emptySlotText;
        [SerializeField] private Image panel;
        [SerializeField] private Transform activeElementContainer;
        public override void display(EquipedCreature element)
        {
            if (element == null) {
                emptySlotText.gameObject.SetActive(true);
                activeElementContainer.gameObject.SetActive(false);
                panel.color = Color.black;
                return;
            }
            creatureImage.sprite = element.getSprite();
            creatureName.text = element.getName();
            nickName.text = element.Nickname;
            rarity.text = element.rarity.ToString();
            equipment.display(element.getEnchantedEquipment());
        }
    }
}


