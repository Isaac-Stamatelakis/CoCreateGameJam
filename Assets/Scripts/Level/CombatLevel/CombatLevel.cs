using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Player;
using LootBoxes;

namespace Levels.Combat {
    [CreateAssetMenu(fileName = "New Combat Level", menuName = "Level/CombatLevel")]
    public class CombatLevel : Level
    {
        
        private List<EquipedCreeture> playerCreatures;
        [SerializeField] public List<EquipedCreeture> enemyCreatures;
        [SerializeField] public List<Lootable> lootables;
        public void initalize(List<EquipedCreeture> playerCreatures) {
            this.playerCreatures = playerCreatures;
        }

        public override void load()
        {
            
        }

        public override string getSceneName()
        {
            return Global.COMBAT_SCENE_NAME;
        }
    }
}

