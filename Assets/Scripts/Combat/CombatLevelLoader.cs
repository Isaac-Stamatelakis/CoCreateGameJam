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
    public class CombatLevelLoader : LevelLoader<CombatLevelObject>
    {
        [SerializeField] private CombatLevelController combatLevelController;
        [SerializeField] private bool useTestCreatures;
        [SerializeField] private List<EquipedCreature> testTeam = new List<EquipedCreature>();
        protected override async void loadLevel(CombatLevelObject level)
        {
            PlayerIO playerIO = PlayerIO.Instance;
            List<EquipedCreature> creatures = playerIO.EquipedCreetures;
            /*
            LootableRegistry registry = LootableRegistry.getInstance();
            EnchantedEquipment mazda5 = new EnchantedEquipment(LootableRegistry.getInstance().getLootable<Equipment>("mazda5"),null);
            List<EquipedCreature> testingCreatings = new List<EquipedCreature> {
                new EquipedCreature(registry.getLootable<Creature>("ambersand"), new List<EnchantedEquipment>()),
                new EquipedCreature(registry.getLootable<Creature>("soddle"), new List<EnchantedEquipment>()),
                new EquipedCreature(registry.getLootable<Creature>("dragoon"), new List<EnchantedEquipment>()),
                new EquipedCreature(registry.getLootable<Creature>("gun_crab"), new List<EnchantedEquipment>{mazda5})
            };
            */
            List<EquipedCreature> playerTeam = PlayerIO.Instance.getPlayerTeam();
            int nonNullCount = GlobalUtils.getNoneNullCount<EquipedCreature>(playerTeam);
            if (nonNullCount==0) {
                nonNullCount = GlobalUtils.getNoneNullCount<EquipedCreature>(testTeam);
                playerTeam = testTeam;
            }
            
            CombatPlayer humanPlayer = new CombatPlayer(playerTeam);
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

