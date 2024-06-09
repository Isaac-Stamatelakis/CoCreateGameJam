using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace UI.Inventory {
    public abstract class CreatureUIDisplay : UIDisplayer<EquipedCreeture>, IPanelDisplay, IPointerClickHandler
    {
        [SerializeField] protected Image panel;
        [SerializeField] protected Image image;
        [SerializeField] protected TextMeshProUGUI levelText;
        protected InventoryUI<EquipedCreeture> inventory;
        protected int index;
        public override void display(EquipedCreeture element, InventoryUI<EquipedCreeture> inventory, int index)
        {
            image.sprite = element.getSprite();
            this.index = index;
            this.inventory = inventory;
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

        public void setPanelColor(Color color)
        {
            panel.color = color;
        }

        public Color getColor()
        {
            return panel.color;
        }
    }
}

