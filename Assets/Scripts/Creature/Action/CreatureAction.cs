using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Levels.Combat;
using Actions.Script;

namespace Actions {
    public enum CreatureSelectionType {
        Ally,
        Enemy,
        Any,
    }
    public static class TargetTypeExtension {
        public static string formatSelection(this CreatureSelectionType targetType, bool allowSelf, bool single) {
            switch (targetType) {
                case CreatureSelectionType.Ally:
                    return single 
                        ? (allowSelf ? "Ally" : "Ally except caster") 
                        : (allowSelf ? "Allies" : "Allies except caster");
                case CreatureSelectionType.Enemy:
                    return single ? "Enemy" : "Enemies";
                case CreatureSelectionType.Any:
                    return single 
                        ? (allowSelf ? "Creature" : "Creature except caster") 
                        : (allowSelf ? "Creatures" : "Creatures except caster");
                default:
                    throw new System.Exception($"Target type {targetType} not covered");
            }
        }

    }
    public abstract class CreatureAction : ScriptedAction
    {
        [SerializeField] private int manaCost;
        public int ManaCost {get => manaCost;}
        public abstract void execute(ManualCreatureSelector selector);
    }

}
