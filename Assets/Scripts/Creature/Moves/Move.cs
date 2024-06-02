using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Levels.Combat;

namespace Creatures.Moves {
    public enum TargetType {
        Allies,
        Enemies
    }
    public abstract class Move : ScriptableObject
    {
        [SerializeField] protected List<TargetType> targetTypes;
        [SerializeField] [Range(1,4)] protected int maxTargets;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected string description;
        public CreatureSelector getSelector(CreatureSelector selector) {
            return new CreatureSelector(
                targetTypes,
                maxTargets
            );
        }
        public abstract void execute(CreatureSelector selector);
    }

}
