using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Creatures;
using Levels.Combat;
using System.Linq;
using System.Globalization;
using TMPro;

namespace Actions {
    public class CreatureSelector
    {
        private CreatureSelectionType targetType;
        private int maxTargets;
        private List<CreatureCombatObject> creatures = new List<CreatureCombatObject>();
        private TextMeshProUGUI textUI;
        private bool random;
        public CreatureSelector(CreatureSelectionType targetType, bool allowSelf, int maxTargets, bool random)
        {
            this.targetType = targetType;
            this.allowSelf = allowSelf;
            this.maxTargets = maxTargets;
            this.random = random;
        }
        public int MaxTargets { get => maxTargets;}
        public CreatureSelectionType TargetType { get => targetType;}
        private bool allowSelf;
        public List<CreatureCombatObject> Creatures { get => creatures; }
        public TextMeshProUGUI TextUI { get => textUI; set => textUI = value; }
        public bool IsFull {get => creatures.Count >= maxTargets;}

        public bool isValidSelection(bool isAlly, bool isSelf) {
            if (!allowSelf && isSelf) {
                return false;
            }
            switch (targetType) {
                case CreatureSelectionType.Ally:
                    if (!isAlly) {
                        return false;
                    }
                    break;
                case CreatureSelectionType.Enemy:
                    if (isAlly) {
                        return false;
                    }
                    break;   
            }
            return true;
        }
        public bool isSatisfied() {
            return creatures.Count > 0;
        }

        public string getTextDescription() {
            if (random) {
                string formattedTarget = targetType.formatSelection(allowSelf,maxTargets==1);
                return $"Targets {maxTargets} Random {formattedTarget}";
            } else {
                string formattedTarget = targetType.formatSelection(allowSelf,false);
                return $"{creatures.Count}/{maxTargets} {formattedTarget} Selected";
            }
            
        }

        public void updateDescription() {
            textUI.text = getTextDescription();
        }

        public void clear() {
            foreach (CreatureCombatObject creatureCombatObject in creatures) {
                if (creatureCombatObject.CreatureInCombat.IsDead) {
                    continue;
                }
                creatureCombatObject.highlight(null);
            }
        }
    }
}

