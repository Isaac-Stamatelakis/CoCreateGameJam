using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Inventory;
using Creatures.Actions;
using UnityEngine.EventSystems;
using TMPro;

namespace Levels.Combat {
    public abstract class CreatureActionUIDisplay : CombatActionUI<CreatureAction>
    {
        [SerializeField] private TextMeshProUGUI manaText;
        public override void display(CreatureAction element, InventoryUI<CreatureAction> inventory, int index)
        {
            base.display(element,inventory,index);
            this.manaText.text = element.ManaCost.ToString();
        }
    }
}

