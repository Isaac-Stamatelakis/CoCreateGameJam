using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Items;
using Items.Consumables;

namespace Levels.Combat {
    public class CombatPlayer
    {
        private List<EquipedCreature> creatures;
        private List<Consumable> consumables;
        public List<EquipedCreature> Creatures {get => creatures;}
        public List<Consumable> Consumables { get => consumables; }

        public CombatPlayer(List<EquipedCreature> equipedCreetures) {
            // Create deep copy
            creatures = new List<EquipedCreature>();
            foreach (EquipedCreature equipedCreature in equipedCreetures) {
                if (equipedCreature == null) {
                    continue;
                }
                
                creatures.Add(new EquipedCreature(
                    equipedCreature.creeture,
                    equipedCreature.EnchantedEquipment,
                    equipedCreature.Health
                ));
            }
        }

        public bool IsDead() {
            return creatures.Count == 0;
        }

        public bool HasCreature(EquipedCreature creature) {
            return creatures.Contains(creature);
        }
        public bool HasCreature(CreatureCombatObject creatureCombatObject) {
            return creatures.Contains(creatureCombatObject.CreatureInCombat.EquipedCreeture);
        }
    }
}

