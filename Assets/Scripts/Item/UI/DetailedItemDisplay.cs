using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
using UnityEngine.UI;
using TMPro;

namespace UI.Inventory {
    public class DetailedItemDisplay : UIDisplayer<Equipment>
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI rarityText;
        [SerializeField] private TextMeshProUGUI statText;
        public override void display(Equipment element, InventoryUI<Equipment> inventory, int index)
        {
            this.titleText.text = element.name;
            this.image.sprite = element.getSprite();
            this.statText.text = "";
            this.rarityText.text = "";
        }
    }
}

