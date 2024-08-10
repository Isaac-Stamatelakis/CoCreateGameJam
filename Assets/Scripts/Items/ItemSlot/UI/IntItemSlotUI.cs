using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Inventory;
using TMPro;
using UnityEngine.UI;

namespace Items {
    public class IntItemSlotUI : UIInventoryDisplayer<IntItemSlot>
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;
        public override void display(IntItemSlot element)
        {
            this.image.sprite = element.getSprite();
            this.text.text = element.Amount.ToString();
        }
    }
}

