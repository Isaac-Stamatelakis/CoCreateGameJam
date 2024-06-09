using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UI.Inventory {
    public abstract class EquipmentUIDisplay : UIDisplayer<Equipment>, IPointerClickHandler
    {
        protected InventoryUI<Equipment> inventory;
        [SerializeField] private Image image;
        protected int index;
        public override void display(Equipment element, InventoryUI<Equipment> inventory, int index)
        {
            if (element == null) {
                this.image.gameObject.SetActive(false);
            } else {
                this.image.gameObject.SetActive(true);
                this.image.sprite = element.getSprite();
            }
            this.inventory = inventory;
            this.index = index;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left) {
                leftClick();
            } else if (eventData.button == PointerEventData.InputButton.Right) {
                rightClick();
            }
        }
        public abstract void leftClick();
        public abstract void rightClick();
    }
}

