using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Inventory;
using UnityEngine.UI;
using TMPro;
using UI;

namespace Items {
    public class StackableItemSlotUI : UIInventoryDisplayer<StackableItemSlot>
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI amountText;
        public override void display(StackableItemSlot element)
        {
            this.image.sprite = element.getSprite();
            this.amountText.text = element.amountToString();
        }
        public void displayWithRequirement(StackableItemSlot element, int currentAmount) {
            this.image.sprite = element.getSprite();
            int requiredAmount = element.amountToInt();
            amountText.text = $"{element.amountToString()}/{currentAmount}";
            amountText.color = requiredAmount < currentAmount ? Color.green : Color.red;
        }
        public void displayWithRequirement(StackableItemSlot element, double currentAmount) {
            this.image.sprite = element.getSprite();
            int requiredAmount = element.amountToInt();
            amountText.text = $"{element.amountToString()}/{currentAmount}";
            amountText.color = requiredAmount < currentAmount ? Color.green : Color.red;
        }
    }
}

