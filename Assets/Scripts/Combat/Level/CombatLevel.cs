using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;

namespace Levels.Combat {
    public class CombatLevel : Level, ICombatLevel
    {
        private List<EquipedCreature> enemyCreatures;
        private LootTable lootTable;

        public CombatLevel(List<EquipedCreature> enemyCreatures, LootTable lootTable)
        {
            this.enemyCreatures = enemyCreatures;
            this.lootTable = lootTable;
        }

        public List<EquipedCreature> getEnemyCreatures()
        {
            return enemyCreatures;
        }

        public ILootTable getLootTable()
        {
            return lootTable;
        }

        public override string getSceneName()
        {
            return Global.COMBAT_SCENE_NAME;
        }
    }
}