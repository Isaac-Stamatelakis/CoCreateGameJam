using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Levels.Combat;
using Actions.Script;

namespace Creatures.Actions {
    public enum TargetType {
        Ally,
        Enemy,
        Any,
        Ally_Not_Self,
        Any_Not_Self,
    }
    public static class TargetTypeExtension {
        public static string formatSelection(this TargetType targetType) {
            switch (targetType) {
                case TargetType.Ally:
                    return "Allies";
                case TargetType.Enemy:
                    return "Enemies";
                case TargetType.Any:
                    return "Creatures";
                case TargetType.Any_Not_Self:
                    return "Creatures except caster";
                case TargetType.Ally_Not_Self:
                    return "Allies except caster";
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
