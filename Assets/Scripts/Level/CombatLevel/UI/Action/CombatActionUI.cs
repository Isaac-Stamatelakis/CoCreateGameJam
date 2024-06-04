using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UI.Inventory;

namespace Levels.Combat {
    public abstract class CombatActionUI<T> : UIDisplayer<T>, IPointerClickHandler where T : ICombatAction
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;
        private T combatAction;
        protected InventoryUI<T> inventory;
        protected int index;
        public override void display(T element, InventoryUI<T> inventory, int index)
        {
            this.inventory = inventory;
            this.index = index;
            this.combatAction = element;
            this.image.sprite = element.getSprite();
            this.text.text = element.getTitle();
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

