using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Creatures;
using Items;
using Items.Equipment;

namespace Levels.Combat {
    public class CombatLevelLoader : LevelLoader<CombatLevel>
    {
        [SerializeField] private CombatLevelController combatLevelController;
        protected override void loadLevel(CombatLevel level)
        {
            PlayerIO playerIO = PlayerIO.Instance;
            List<EquipedCreeture> creatures = playerIO.EquipedCreetures;
            LootableRegistry registry = LootableRegistry.getInstance();
            List<EquipedCreeture> testingCreatings = new List<EquipedCreeture> {
                new EquipedCreeture(registry.getLootable<Creature>("clockadoodle"), new List<Equipment>()),
                new EquipedCreeture(registry.getLootable<Creature>("soddle"), new List<Equipment>()),
                new EquipedCreeture(registry.getLootable<Creature>("dragoon"), new List<Equipment>())
            };
            CombatPlayer humanPlayer = new CombatPlayer(testingCreatings);
            CombatPlayer aiPlayer = new CombatPlayer(level.enemyCreatures);
            combatLevelController.load(humanPlayer,aiPlayer,level);
        }
    }
}

