using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Levels.Combat;

namespace Creatures.Actions {
    public enum TargetType {
        Allies,
        Enemies
    }
    public abstract class CreatureAction : ScriptableObject, ICombatAction
    {
        [SerializeField] protected List<TargetType> targetTypes;
        [SerializeField] [Range(1,4)] protected int maxTargets;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected int manaCost;
        [SerializeField] protected string description;
        public int ManaCost {get => manaCost;}
        public CreatureSelector getSelector(CreatureSelector selector) {
            return new CreatureSelector(
                targetTypes,
                maxTargets
            );
        }
        public abstract void execute(CreatureSelector selector);

        public string getTitle()
        {
            return name;
        }

        public Sprite getSprite()
        {
            return icon;
        }

        public abstract string getDescription();
    }

}
