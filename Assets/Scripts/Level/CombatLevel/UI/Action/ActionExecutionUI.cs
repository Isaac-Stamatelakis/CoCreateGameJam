using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
        public void display(ICombatAction combatAction, CombatLevelActionUIController combatLevelActionUIController) {
            this.title.text = combatAction.getTitle();
            this.icon.sprite = combatAction.getSprite();
            this.description.text = combatAction.getDescription();
            this.combatLevelActionUIController = combatLevelActionUIController;
            backButton.onClick.AddListener(() => {
                GameObject.Destroy(gameObject);

            });
            executionButton.onClick.AddListener(() => {

            });
        }

        public void OnDestroy() {
            backButton.onClick.RemoveAllListeners();
            executionButton.onClick.RemoveAllListeners();
        }
    }

}
