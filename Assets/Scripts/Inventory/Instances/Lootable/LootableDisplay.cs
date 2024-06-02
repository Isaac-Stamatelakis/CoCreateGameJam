using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UI.Inventory {
    public class LootableDisplay : UIDisplayer<Lootable>
    {
        [SerializeField] private Image image;
        public override void display(Lootable element, InventoryUI<Lootable> inventory, int index)
        {
            image.sprite = element.getSprite();
        }
    }
}

