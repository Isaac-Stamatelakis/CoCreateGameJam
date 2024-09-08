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
            CombatPlayer humanPlayer;
            if (useTestCreatures) {
                humanPlayer = new CombatPlayer(testTeam);
            } else {
                List<EquipedCreature> playerTeam = PlayerIO.Instance.getPlayerTeam();
                humanPlayer = new CombatPlayer(playerTeam);
            }
            
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
                EquipedCreature equipedCreature = creatureInCombat.EquipedCreeture;
                loadTasks.Add(actionRegistry.loadActions(equipedCreature.creeture.Id, ActionBundleType.Creature));
                foreach (EnchantedEquipment enchantedEquipment in equipedCreature.EnchantedEquipment) {
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

