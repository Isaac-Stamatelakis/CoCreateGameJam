using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Levels.Combat;
using Actions.Script;

namespace Creatures.Actions {
    public enum CreatureSelectionType {
        Ally,
        Enemy,
        Any,
    }
    public static class TargetTypeExtension {
        public static string formatSelection(this CreatureSelectionType targetType, bool allowSelf) {
            switch (targetType) {
                case CreatureSelectionType.Ally:
                    return allowSelf ? "Allies" : "Allies except caster";
                case CreatureSelectionType.Enemy:
                    return "Enemies";
                case CreatureSelectionType.Any:
                    return allowSelf ? "Creatures" : "Creatures except caster";
                default:
                    throw new System.Exception($"Target type {targetType} not covered");
            }
        }
    }
    public abstract class CreatureAction : ScriptedAction
    {
        [SerializeField] private int manaCost;
        public int ManaCost {get => manaCost;}
        public abstract void execute(CreatureSelector selector);
    }

}
