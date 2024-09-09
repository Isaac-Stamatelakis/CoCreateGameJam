using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Player;
using LootBoxes;
using Items;

namespace Levels.Combat {
    [CreateAssetMenu(fileName = "New Combat Level", menuName = "Level/CombatLevel")]
    public class CombatLevelObject : LevelObject, ICombatLevel
    {
        [SerializeField] public List<EquipedCreature> enemyCreatures;
        [SerializeField] public LootTableObject lootTable;

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

