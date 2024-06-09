using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI.Inventory;
using Creatures.Actions;

namespace Levels.Combat {
    public interface ISelectionActionList {
        public void selectAction(ICombatAction combatAction);
    }
    public class SelectionCreatureActionList : InventoryUI<CreatureAction>, ISelectionActionList
    {
        [SerializeField] private ActionSelectUI actionSelectUI;
        public void selectAction(ICombatAction combatAction)
        {
            
        }
    }
}

