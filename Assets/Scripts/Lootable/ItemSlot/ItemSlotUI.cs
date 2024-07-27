using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Inventory;
using UnityEngine.UI;
using TMPro;

namespace Items {
    public class ItemSlotUI : UIDisplayer<ItemSlot>
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI amountText;
        public override void display(ItemSlot element)
        {
            display(element.getSprite(),element.Amount);
        }
        public void display(Sprite sprite, int amount) {
            this.image.sprite = sprite;
            if (amount <= 1) {
                amountText.text = "";
            } else {
                amountText.text = amount.ToString();
            }
        }
    }
}

