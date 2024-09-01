using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UI.Inventory;
using Creatures;

namespace Levels.Combat {
    public class EnemyTurnUI : UIInventoryDisplayer<EquipedCreeture>
    {
        [SerializeField] private Image creatureImage;
        [SerializeField] private TextMeshProUGUI text;

        public override void display(EquipedCreeture element)
        {
            creatureImage.sprite = element.getSprite();
            text.text = CombatLevelUtils.getEnemyTurnText(element.getName());
        }
    }
}

