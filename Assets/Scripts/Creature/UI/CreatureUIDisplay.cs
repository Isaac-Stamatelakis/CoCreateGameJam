using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace UI.Inventory {
    public class CreatureUIDisplay : UIInventoryDisplayer<EquipedCreeture>, IPointerClickHandler
    {
        [SerializeField] protected Image image;
        [SerializeField] protected TextMeshProUGUI levelText;
        public override void display(EquipedCreeture element)
        {
            image.sprite = element.getSprite();
        }
    }
}

