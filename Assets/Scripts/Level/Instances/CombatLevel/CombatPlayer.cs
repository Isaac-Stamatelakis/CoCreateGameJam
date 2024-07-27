using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Items;
using Items.Consumables;

namespace Levels.Combat {
    public class CombatPlayer
    {
        private List<CreatureInCombat> creatures;
        private List<Consumable> consumables;
        public List<CreatureInCombat> Creatures {get => creatures;}
        public List<Consumable> Consumables { get => consumables; }

        public CombatPlayer(List<EquipedCreeture> equipedCreetures) {
            creatures = new List<CreatureInCombat>();
            foreach (EquipedCreeture creeture in equipedCreetures) {
                if (creeture == null) {
                    continue;
                }
                creatures.Add(new CreatureInCombat(creeture));
            }
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

