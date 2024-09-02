using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UI.Lists;

namespace Levels.Combat {
    public class ActionUIElement : UIInventoryDisplayer<ICombatAction>, IPointerClickHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI number;
        protected ICombatAction combatAction;
        public override void display(ICombatAction element)
        {
            this.combatAction = element;
            this.image.sprite = element.getSprite();
            this.title.text = element.getTitle();
            if (element is IManaCombatAction manaCombatAction) {
                number.text = manaCombatAction.getManaCost().ToString();
            } else {
                number.gameObject.SetActive(false);
            }
        }
    }
}

