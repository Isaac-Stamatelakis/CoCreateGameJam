using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Actions.Script;
using Creatures.Actions;

namespace Levels.Combat {
    public class ActionExecutionUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI selectionText;
        [SerializeField] private Button backButton;
        [SerializeField] private Button executionButton;
        private CombatLevelActionUIController combatLevelActionUIController;
        private CreatureSelector creatureSelector;
        private CommandExecutionState commandExecutionState;
        public void display(ICombatAction combatAction, CombatLevelActionUIController combatLevelActionUIController) {
            if (combatAction is ScriptedAction scriptedAction) {
                commandExecutionState = new CommandExecutionState(scriptedAction);
                commandExecutionState.executeSection();
                creatureSelector = commandExecutionState.getCurrentSelector();
                selectionText.text = creatureSelector.getTextDescription();
                combatLevelActionUIController.CombatLevelUIController.CombatLevelController.CreatureHighlightController.setSelector(creatureSelector);
            }
            this.title.text = combatAction.getTitle();
            this.icon.sprite = combatAction.getSprite();
            this.description.text = combatAction.getDescription();
            this.combatLevelActionUIController = combatLevelActionUIController;
            backButton.onClick.AddListener(() => {
                combatLevelActionUIController.ActionSelectUI.gameObject.SetActive(true);
                GameObject.Destroy(gameObject);
            });
            executionButton.onClick.AddListener(() => {
                Debug.Log(creatureSelector.isSatisfied());
                if (creatureSelector.isSatisfied()) {
                    commandExecutionState.executeSection();
                }
            });
        }

        public void OnDestroy() {
            backButton.onClick.RemoveAllListeners();
            executionButton.onClick.RemoveAllListeners();
        }
    }

}
