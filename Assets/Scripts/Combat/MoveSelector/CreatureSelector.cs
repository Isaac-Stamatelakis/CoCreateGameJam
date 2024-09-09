using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Creatures;
using Levels.Combat;
using System.Linq;
using System.Globalization;
using TMPro;
using Actions.Script;

namespace Actions {
    public interface ICreatureSelector {
        public ICombatCreature getTarget(int iteration);
        public int maxIterations();
    }
    public class CreatureSelector<T> : ICreatureSelector where T : ICombatCreature
    {
        protected List<T> creatures;
        public CreatureSelector(List<T> creatures)
        {
            this.creatures = creatures;
        }

        public List<T> getCreatures()
        {
            return creatures;
        }

        public ICombatCreature getTarget(int iteration)
        {
            return (ICombatCreature)creatures[iteration];
        }

        public int maxIterations()
        {
            return creatures.Count;
        }
    }
    public interface IDescriptableSelector {
        public void setTextElement(TextMeshProUGUI uiElement);
        public string getDescription();
        public void updateDescription();
    }
    public interface IRequirementSelector {
        public bool isSatisfied();
    }

    public abstract class InteractableCreatureSelector : CreatureSelector<CreatureCombatObject>, IDescriptableSelector
    {
        protected CreatureSelectionType targetType;
        protected int maxTargets;
        protected bool allowSelf;
        private TextMeshProUGUI textUI;
        public int MaxTargets { get => maxTargets;}
        public InteractableCreatureSelector(List<CreatureCombatObject> creatures, CreatureSelectionType targetType, bool allowSelf, int maxTargets) : base(creatures)
        {
            this.targetType = targetType;
            this.allowSelf = allowSelf;
            this.maxTargets = maxTargets;
        }

        public void setTextElement(TextMeshProUGUI uiElement)
        {
            textUI = uiElement;
        }

        public abstract string getDescription();

        public void updateDescription()
        {
            textUI.text = getDescription();
        }
    }
    public class ManualCreatureSelector : InteractableCreatureSelector, IRequirementSelector
    {
        public ManualCreatureSelector(CreatureSelectionType targetType, bool allowSelf, int maxTargets) : base(new List<CreatureCombatObject>(), targetType, allowSelf, maxTargets)
        {
        }

        public List<CreatureCombatObject> Creatures { get => creatures; set => creatures=value;}
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
        public override string getDescription()
        {
            string formattedTarget = targetType.formatSelection(allowSelf,false);
            return $"{creatures.Count}/{maxTargets} {formattedTarget} Selected";
        }
    }
    public class DescriptableRandomSelector : InteractableCreatureSelector, IDescriptableSelector
    {
        public DescriptableRandomSelector(List<CreatureCombatObject> creatures, CreatureSelectionType targetType, bool allowSelf, int maxTargets) : base(creatures, targetType, allowSelf, maxTargets)
        {
        }

        public override string getDescription()
        {
            string formattedTarget = targetType.formatSelection(allowSelf,maxTargets==1);
            return $"Targets {maxTargets} Random {formattedTarget}";
        }

        
    }
}

