using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Creatures;
using Items;
using Items.Equipment;
using Actions;
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
            EnchantedEquipment mazda5 = new EnchantedEquipment(LootableRegistry.getInstance().getLootable<Equipment>("mazda5"),null);
            List<EquipedCreeture> testingCreatings = new List<EquipedCreeture> {
                new EquipedCreeture(registry.getLootable<Creature>("ambersand"), new List<EnchantedEquipment>()),
                new EquipedCreeture(registry.getLootable<Creature>("soddle"), new List<EnchantedEquipment>()),
                new EquipedCreeture(registry.getLootable<Creature>("dragoon"), new List<EnchantedEquipment>()),
                new EquipedCreeture(registry.getLootable<Creature>("gun_crab"), new List<EnchantedEquipment>{mazda5})
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
            ActionRegistry actionRegistry = ActionRegistry.getInstance();
            var loadTasks = new List<Task>();
            foreach (CreatureInCombat creatureInCombat in combatPlayer.Creatures) {
                loadTasks.Add(actionRegistry.loadActions(creatureInCombat.EquipedCreeture.creeture.Id, ActionBundleType.Creature));
                foreach (EnchantedEquipment enchantedEquipment in creatureInCombat.EquipedCreeture.EnchantedEquipment) {
                    if (enchantedEquipment == null || enchantedEquipment.getId() == null) {
                        continue;
                    }
                    loadTasks.Add(actionRegistry.loadActions(enchantedEquipment.getId(),ActionBundleType.Equipment));
                }
            }
            await Task.WhenAll(loadTasks);
        }
    }
}

