using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items.Equipment;
using UnityEngine.UI;
using TMPro;

namespace UI.Lists {
    public class DetailedItemDisplay : UIInventoryDisplayer<Equipment>
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI rarityText;
        [SerializeField] private TextMeshProUGUI statText;
        public override void display(Equipment element)
        {
            this.titleText.text = element.getName();
            this.image.sprite = element.getSprite();
            this.statText.text = "";
            this.rarityText.text = "";
        }
    }
}

