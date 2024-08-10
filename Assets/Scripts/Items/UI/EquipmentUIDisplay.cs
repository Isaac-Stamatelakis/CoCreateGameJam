using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UI.Inventory {
    public class EquipmentUIDisplay : UIInventoryDisplayer<Equipment>, IPointerClickHandler
    {
        [SerializeField] private Image image;
        public override void display(Equipment element)
        {
            if (element == null) {
                this.image.gameObject.SetActive(false);
            } else {
                this.image.gameObject.SetActive(true);
                this.image.sprite = element.getSprite();
            }
        }
    }
}

