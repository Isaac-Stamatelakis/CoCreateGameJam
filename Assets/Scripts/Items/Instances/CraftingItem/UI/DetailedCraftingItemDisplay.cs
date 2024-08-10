using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Inventory;
using UnityEngine.UI;
using TMPro;

namespace Items {
    public class DetailedCraftingItemDisplay : UIInventoryDisplayer<StackableItemSlot>
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private TextMeshProUGUI description;
        public override void display(StackableItemSlot element)
        {
            if (element.Lootable is not CraftingItem craftingItem) {
                return;
            }
            nameText.text = craftingItem.name;
            amountText.text = element.amountToString();
            description.text = craftingItem.Description;
            image.sprite = element.getSprite();
        }
    }

}
