using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Items;

namespace Levels.Combat {
    public class CombatPlayer
    {
        private List<CreatureInCombat> creatures;
        public List<CreatureInCombat> Creatures {get => creatures;}
        private Dictionary<CreatureInCombat, CreatureCombatObject> creatureObjectDict;
        public CombatPlayer(List<EquipedCreeture> equipedCreetures) {
            creatures = new List<CreatureInCombat>();
            foreach (EquipedCreeture creeture in equipedCreetures) {
                if (creeture == null) {
                    continue;
                }
                creatures.Add(new CreatureInCombat(creeture));
            }
        }
        public CreatureCombatObject getCreatureObject(CreatureInCombat creature) {
            if (creatureObjectDict.ContainsKey(creature)) {
                return creatureObjectDict[creature];
            }
            return null;
        }

        public bool IsDead() {
            foreach (CreatureInCombat creatureInCombat in creatures) {
                if (creatureInCombat.Health > 0) {
                    return false;
                }
            }
            return true;
        }

        public bool HasCreature(CreatureInCombat creature) {
            return creatures.Contains(creature);
        }
    }
}

