using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Actions.Script;
using Actions;

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
        private IDescriptableSelector creatureSelector;
        private LiveCommandExecutionState commandExecutionState;
        public void Start() {
            executionButton.onClick.AddListener(() => {
                if (creatureSelector is IRequirementSelector requirementSelector) {
                    if (!requirementSelector.isSatisfied()) {
                        return;
                    }
                }
                StartCoroutine(execute());
            });
        }

        public IEnumerator execute() {
            yield return StartCoroutine(commandExecutionState.executeSection());
            yield return StartCoroutine(combatLevelController.nextCreatureTurn());
            GameObject.Destroy(gameObject);
        }

        private void displaySelector() {
            creatureSelector = (IDescriptableSelector) commandExecutionState.CreatureSelector;
            creatureSelector.setTextElement(selectionText);
            creatureSelector.updateDescription();
            if (creatureSelector is ManualCreatureSelector manualCreatureSelector) {
                combatLevelController.CreatureHighlightController.setSelector(manualCreatureSelector);
            }
        }

        public void display(ICombatAction combatAction, CreatureCombatObject creatureCombatObject, CombatLevelActionUIController combatLevelActionUIController) {
            combatLevelController = combatLevelActionUIController.CombatLevelUIController.CombatLevelController;
            if (combatAction is ScriptedAction scriptedAction) {
                commandExecutionState = new LiveCommandExecutionState(scriptedAction,creatureCombatObject,true);
                StartCoroutine(commandExecutionState.executeSection());
                displaySelector();
            }
            this.title.text = combatAction.getTitle();
            this.icon.sprite = combatAction.getSprite();
            this.description.text = combatAction.getDescription();
            this.combatLevelActionUIController = combatLevelActionUIController;
            backButton.onClick.AddListener(() => {
                combatLevelActionUIController.ActionSelectUI.gameObject.SetActive(true);
                combatLevelController.CreatureHighlightController.setSelector(null);
                GameObject.Destroy(gameObject);
            });
            
        }

        public void OnDestroy() {
            backButton.onClick.RemoveAllListeners();
            executionButton.onClick.RemoveAllListeners();
        }
    }

}
