using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Creatures;
using Items;
using Items.Equipment;
using System.Threading.Tasks;

namespace Levels.Combat {
    public class CombatLevelLoader : LevelLoader<CombatLevel>
    {
        [SerializeField] private CombatLevelController combatLevelController;
        protected override async void loadLevel(CombatLevel level)
        {
            PlayerIO playerIO = PlayerIO.Instance;
            List<EquipedCreeture> creatures = playerIO.EquipedCreetures;
            LootableRegistry registry = LootableRegistry.getInstance();
            List<EquipedCreeture> testingCreatings = new List<EquipedCreeture> {
                new EquipedCreeture(registry.getLootable<Creature>("clockadoodle"), new List<EnchantedEquipment>()),
                new EquipedCreeture(registry.getLootable<Creature>("soddle"), new List<EnchantedEquipment>()),
                new EquipedCreeture(registry.getLootable<Creature>("dragoon"), new List<EnchantedEquipment>())
            };
            CombatPlayer humanPlayer = new CombatPlayer(testingCreatings);
            CombatPlayer aiPlayer = new CombatPlayer(level.enemyCreatures);
            await Task.WhenAll(
                loadPlayerCreatureActions(humanPlayer),
                loadPlayerCreatureActions(aiPlayer)
            );
            combatLevelController.load(humanPlayer,aiPlayer,level);
        }

        private async Task loadPlayerCreatureActions(CombatPlayer combatPlayer) {
            CreatureActionRegistry creatureActionRegistry = CreatureActionRegistry.getInstance();
            foreach (CreatureInCombat creatureInCombat in combatPlayer.Creatures) {
                await creatureActionRegistry.loadActions(creatureInCombat.EquipedCreeture.creeture.Id);
            }
        }
    }
}

