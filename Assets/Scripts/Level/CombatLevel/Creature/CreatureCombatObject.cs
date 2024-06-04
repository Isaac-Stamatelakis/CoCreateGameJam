using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;

namespace Levels.Combat {
    public enum HighlightType {
        View,
        Turn,
        Enemy,
        Ally
    }
    public class CreatureCombatObject : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer highlightSprite;
        private CreatureInCombat creatureInCombat;
        private CreatureCombatUI combatUI;
        public CreatureInCombat CreatureInCombat { get => creatureInCombat; }

        public void display(CreatureInCombat creatureInCombat) {
            this.creatureInCombat = creatureInCombat;
            animator.runtimeAnimatorController = creatureInCombat.EquipedCreeture.Creeture.AnimationController;
        }

        public void syncCombatUI(CreatureCombatUI creatureCombatUI) {
            this.combatUI = creatureCombatUI;
        }

        public void highlight(HighlightType? highlightType) {
            switch (highlightType) {
                case HighlightType.Turn:
                    changeHighlightColor(Color.cyan);
                    break;
                case HighlightType.Enemy:
                    changeHighlightColor(Color.red);
                    break;
                case HighlightType.Ally:
                    changeHighlightColor(Color.green);
                    break;
                case HighlightType.View:
                    changeHighlightColor(Color.yellow);
                    break;
                default:
                    highlightSprite.gameObject.SetActive(false);
                    break;
            }
        }

        private void changeHighlightColor(Color color) {
            highlightSprite.gameObject.SetActive(true);
            highlightSprite.color = color;
        }
    }
}

