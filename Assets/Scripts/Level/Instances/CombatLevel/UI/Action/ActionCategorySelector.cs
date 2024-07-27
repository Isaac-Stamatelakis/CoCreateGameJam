using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Levels.Combat {
    public class ActionCategorySelector : MonoBehaviour
    {
        [SerializeField] private ActionSelectUI actionSelectUIController;
        [SerializeField] private Button creatureButton;
        [SerializeField] private Button consumableButton;
        [SerializeField] private Button otherButton;
        public void Start() {
            creatureButton.onClick.AddListener(() => {
                actionSelectUIController.displayActionType(ActionType.Creature);
            });
            consumableButton.onClick.AddListener(() => {
                actionSelectUIController.displayActionType(ActionType.Consumable);
            });
            otherButton.onClick.AddListener(() => {
                actionSelectUIController.displayActionType(ActionType.Other);
            });
        }
    }
}

