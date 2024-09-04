using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using UnityEngine.UI;
using TMPro;
using UI.Lists;

namespace Creatures.UI {
    public class CreatureUIDisplay : UIInventoryDisplayer<EquipedCreature>
    {
        [SerializeField] protected Image image;
        [SerializeField] protected TextMeshProUGUI levelText;
        public override void display(EquipedCreature element)
        {
            image.sprite = element.getSprite();
        }
    }
}

