using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;
using Creatures;
using Items.Consumables;
using UI.Lists;
using Actions;
using UnityEngine.UI;
using System.Linq;

namespace Levels.Combat {
    public class ActionSelectUI : MonoBehaviour
    {
        [SerializeField] private InventoryUI<ICombatAction> dynamicActionDisplay;
        [SerializeField] private GridLayoutGroup otherActionDisplay;
        private ActionType? currentActionType = null;
        private CreatureCombatObject creatureCombatObject;
        private List<Consumable> consumables;
        public CreatureCombatObject CreatureCombatObject { get => creatureCombatObject;}

        public void display(CreatureCombatObject creatureCombatObject, List<Consumable> consumables) {
            gameObject.SetActive(true);
            this.currentActionType = null;
            this.creatureCombatObject = creatureCombatObject;
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
                    CreatureActionCollection creatureActionCollection = ActionRegistry.getInstance().getAction<CreatureActionCollection>(CreatureCombatObject.CreatureInCombat.EquipedCreeture.creeture.Id);
                    dynamicActionDisplay.display(creatureActionCollection.actions.Cast<ICombatAction>().ToList());
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

