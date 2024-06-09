using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Creatures;
using Items;

namespace Levels.Combat {
    public class CombatLevelLoader : LevelLoader<CombatLevel>
    {
        [SerializeField] private CombatLevelController combatLevelController;
        protected override void loadLevel(CombatLevel level)
        {
            PlayerIO playerIO = PlayerIO.Instance;
            List<EquipedCreeture> creatures = playerIO.EquipedCreetures;
            CreatureRegistry creatureRegistry = CreatureRegistry.getInstance();
            List<EquipedCreeture> testingCreatings = new List<EquipedCreeture> {
                new EquipedCreeture(creatureRegistry.getCreature("clockadoodle"), new List<Equipment>()),
                new EquipedCreeture(creatureRegistry.getCreature("goof_ball"), new List<Equipment>())
            };
            foreach (EquipedCreeture equipedCreeture in testingCreatings) {
                Debug.Log(equipedCreeture.creeture.name);
            }
            CombatPlayer humanPlayer = new CombatPlayer(testingCreatings);
            CombatPlayer aiPlayer = new CombatPlayer(level.enemyCreatures);
            combatLevelController.load(humanPlayer,aiPlayer,level);
        }
    }
}

