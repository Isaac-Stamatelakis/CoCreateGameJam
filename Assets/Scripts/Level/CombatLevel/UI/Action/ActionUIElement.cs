using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UI.Inventory;

namespace Levels.Combat {
    public abstract class ActionUIElement : UIDisplayer<ICombatAction>, IPointerClickHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI number;
        protected ICombatAction combatAction;
        protected InventoryUI<ICombatAction> inventory;
        protected int index;
        public override void display(ICombatAction element, InventoryUI<ICombatAction> inventory, int index)
        {
            this.inventory = inventory;
            this.index = index;
            this.combatAction = element;
            this.image.sprite = element.getSprite();
            this.title.text = element.getTitle();
            if (element is IManaCombatAction manaCombatAction) {
                number.text = manaCombatAction.getManaCost().ToString();
            } else {
                number.gameObject.SetActive(false);
            }
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

