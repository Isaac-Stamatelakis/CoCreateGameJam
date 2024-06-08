using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Creatures;
using Levels.Combat;
using System.Linq;
using System.Globalization;

namespace Creatures.Actions {
    public class CreatureSelector
    {
        private TargetType targetType;
        private int maxTargets;
        private List<CreatureCombatObject> creatures = new List<CreatureCombatObject>();
        public CreatureSelector(TargetType targetType, int maxTargets)
        {
            this.targetType = targetType;
            this.maxTargets = maxTargets;
        }
        public int MaxTargets { get => maxTargets;}
        public TargetType TargetType { get => targetType;}
        public List<CreatureCombatObject> Creatures { get => creatures; }
        public bool isSatisfied() {
            return creatures.Count > 0;
        }

        public string getTextDescription() {
            string formattedTarget = targetType.formatSelection();
            return $"{creatures.Count}/{maxTargets} {formattedTarget} Selected";
        }
    }
}

