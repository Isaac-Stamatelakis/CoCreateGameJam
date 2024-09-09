using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Items;
using Items.Consumables;

namespace Levels.Combat {
    public class CombatPlayer
    {
        private List<CreatureInCombat> creatures = new List<CreatureInCombat>();
        private List<Consumable> consumables;
        public List<CreatureInCombat> Creatures {get => creatures;}
        public List<Consumable> Consumables { get => consumables; }
        public CombatPlayer(List<EquipedCreature> equipedCreetures) {
            foreach (EquipedCreature equipedCreature in equipedCreetures) {
                if (equipedCreature == null) {
                    continue;
                }
                creatures.Add(new CreatureInCombat(equipedCreature));
            }
        }
        public CombatPlayer(List<CreatureInCombat> combatCreatures) {
            this.creatures = combatCreatures;
        }

        public bool IsDead() {
            return creatures.Count == 0;
        }

        public bool HasCreature(CreatureInCombat creature) {
            return creatures.Contains(creature);
        }
        public bool HasCreature(CreatureCombatObject creatureCombatObject) {
            return creatures.Contains(creatureCombatObject.CreatureInCombat);
        }
    }
}

