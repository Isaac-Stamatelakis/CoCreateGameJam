using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Creatures;
using Levels.Combat;

namespace Creatures.Moves {
    public interface IMultiTypeCreatureSelector {
        public void AddCreature(TargetType targetType, CreatureCombatObject creature);
        public List<CreatureCombatObject> GetCreaturesOfCategory(TargetType targetType);
    }
    public class CreatureSelector : IMoveCreatureSelector, IMultiTypeCreatureSelector
    {
        private HashSet<TargetType> validTargets = new HashSet<TargetType>();
        private int maxTargets;
        private Dictionary<TargetType, List<CreatureCombatObject>> creatures = new Dictionary<TargetType, List<CreatureCombatObject>>();
        private Queue<(TargetType, CreatureCombatObject)> selectionHistory = new Queue<(TargetType, CreatureCombatObject)>();

        public CreatureSelector(List<TargetType> validTargetList, int maxTargets)
        {
            foreach (TargetType targetType in validTargetList) {
                validTargets.Add(targetType);
            }
            this.maxTargets = maxTargets;
        }

        public void AddCreature(TargetType targetType, CreatureCombatObject creature)
        {
            if (selectionHistory.Count > maxTargets) {
                (TargetType,CreatureCombatObject) lastAdded = selectionHistory.Dequeue();
                creatures[lastAdded.Item1].Remove(lastAdded.Item2);
            }
            if (!creatures.ContainsKey(targetType)) {
                creatures[targetType] = new List<CreatureCombatObject>();
            }
            creatures[targetType].Add(creature);
            selectionHistory.Enqueue((targetType,creature));
        }

        public List<CreatureCombatObject> GetCreatures()
        {
            List<CreatureCombatObject> list = new List<CreatureCombatObject>();
            foreach (List<CreatureCombatObject> creatureList in creatures.Values) {
                list.AddRange(creatureList);
            }
            return list;
        }

        public List<CreatureCombatObject> GetCreaturesOfCategory(TargetType val) {
            if (!creatures.ContainsKey(val)) {
                return null;
            }
            return creatures[val];
        }
    }
}

