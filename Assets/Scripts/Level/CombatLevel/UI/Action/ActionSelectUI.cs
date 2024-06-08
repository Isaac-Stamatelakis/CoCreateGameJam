using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;
using Creatures;
using Items.Consumables;
using UI.Inventory;
using Creatures.Actions;
using UnityEngine.UI;

namespace Levels.Combat {
    public class ActionSelectUI : MonoBehaviour
    {
        [SerializeField] private InventoryUI<ICombatAction> dynamicActionDisplay;
        [SerializeField] private GridLayoutGroup otherActionDisplay;
        private ActionType? currentActionType = null;
        private CreatureInCombat creatureInCombat;
        private List<Consumable> consumables;
        public void display(CreatureInCombat creatureInCombat, List<Consumable> consumables) {
            gameObject.SetActive(true);
            this.creatureInCombat = creatureInCombat;
            this.consumables = consumables;
            displayActionType(ActionType.Creature);
        }
        public void displayActionType(ActionType actionType) {
            if (actionType == currentActionType) {
                return;
            }
            currentActionType = actionType;
            switch (actionType) {
                case ActionType.Creature:
                    dynamicActionDisplay.gameObject.SetActive(true);
                    otherActionDisplay.gameObject.SetActive(false);
                    List<ICombatAction> actions = new List<ICombatAction>();
                    actions.AddRange(creatureInCombat.EquipedCreeture.creeture.Actions);
                    dynamicActionDisplay.display(actions);
                    break;
                case ActionType.Consumable:
                    dynamicActionDisplay.gameObject.SetActive(true);
                    otherActionDisplay.gameObject.SetActive(false);
                    break;
                case ActionType.Other:
                    dynamicActionDisplay.gameObject.SetActive(false);
                    otherActionDisplay.gameObject.SetActive(true);
                    break;
            }
        }
    }

    public enum ActionType {
        Creature,
        Consumable,
        Other
    }
}

