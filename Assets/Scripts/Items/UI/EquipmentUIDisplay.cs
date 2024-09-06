using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Items.Equipment;

namespace UI.Lists {
    public class EquipmentUIDisplay : UIInventoryDisplayer<EnchantedEquipment>, IPointerClickHandler
    {
        [SerializeField] private Image image;
        public override void display(EnchantedEquipment element)
        {
            if (element == null || element.Equipment == null) {
                this.image.gameObject.SetActive(false);
            } else {
                this.image.gameObject.SetActive(true);
                this.image.sprite = element.getSprite();
            }
        }
    }
}

