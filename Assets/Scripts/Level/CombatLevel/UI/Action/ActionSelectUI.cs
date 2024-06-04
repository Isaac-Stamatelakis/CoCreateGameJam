using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;
using Creatures;
using Items.Consumables;

namespace Levels.Combat {
    public class ActionSelectUI : MonoBehaviour
    {
        private ActionType currentActionType;
        private CreatureInCombat creatureInCombat;
        private List<Consumable> consumables;
        public void display(CreatureInCombat creatureInCombat, List<Consumable> consumables) {
            this.creatureInCombat = creatureInCombat;
            this.consumables = consumables;
        }
        public void displayActionType(ActionType actionType) {
            if (actionType == currentActionType) {
                return;
            }
            switch (actionType) {
                case ActionType.Creature:
                    break;
                case ActionType.Consumable:
                    break;
                case ActionType.Other:
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

