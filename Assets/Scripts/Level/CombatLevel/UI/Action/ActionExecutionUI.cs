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
        private CombatLevelController combatLevelController;
        private CreatureSelector creatureSelector;
        private CommandExecutionState commandExecutionState;
        public void Start() {
            executionButton.onClick.AddListener(() => {
                if (creatureSelector.isSatisfied()) {
                    StartCoroutine(execute());
                }
            });
        }

        public IEnumerator execute() {
            yield return StartCoroutine(commandExecutionState.executeSection());
            if (commandExecutionState.Complete) {
                yield return StartCoroutine(combatLevelController.nextCreatureTurn());
                GameObject.Destroy(gameObject);
            } else {
                displaySelector();
            }
        }

        private void displaySelector() {
            creatureSelector = commandExecutionState.getCurrentSelector();
            creatureSelector.TextUI = selectionText;
            creatureSelector.updateDescription();
            combatLevelController.CreatureHighlightController.setSelector(creatureSelector);
        }
        public void display(ICombatAction combatAction, CombatLevelActionUIController combatLevelActionUIController) {
            combatLevelController = combatLevelActionUIController.CombatLevelUIController.CombatLevelController;
            if (combatAction is ScriptedAction scriptedAction) {
                CreatureCombatObject currentlyMovingCreature = combatLevelController.getCurrentlyMovingCreature();
                commandExecutionState = new CommandExecutionState(scriptedAction,currentlyMovingCreature);
                StartCoroutine(commandExecutionState.executeSection());
                displaySelector();
            }
            this.title.text = combatAction.getTitle();
            this.icon.sprite = combatAction.getSprite();
            this.description.text = combatAction.getDescription();
            this.combatLevelActionUIController = combatLevelActionUIController;
            backButton.onClick.AddListener(() => {
                combatLevelActionUIController.ActionSelectUI.gameObject.SetActive(true);
                GameObject.Destroy(gameObject);
            });
            
        }

        public void OnDestroy() {
            backButton.onClick.RemoveAllListeners();
            executionButton.onClick.RemoveAllListeners();
        }
    }

}
