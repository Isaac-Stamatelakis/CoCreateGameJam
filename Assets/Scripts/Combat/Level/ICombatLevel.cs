using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;

namespace Levels.Combat {
    public interface ICombatLevel {
        public List<EquipedCreature> getEnemyCreatures();
        public ILootTable getLootTable();
    }
}