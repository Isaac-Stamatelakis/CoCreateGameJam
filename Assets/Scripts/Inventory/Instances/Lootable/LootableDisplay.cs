using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace UI.Inventory {
    public class LootableDisplay : UIInventoryDisplayer<LootableCount>
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI amount;
        public override void display(LootableCount element, InventoryUI<LootableCount> inventory, int index)
        {
            image.sprite = element.getSprite();
            amount.text = element.getNumberHeader();
        }
    }
}

