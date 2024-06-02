using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Creatures;

namespace Levels.Combat {
    public class CombatLevelLoader : LevelLoader<CombatLevel>
    {
        [SerializeField] private CombatLevelController combatLevelController;
        [SerializeField] private CombatPlayerInputController inputController;

        protected override void loadLevel(CombatLevel level)
        {
            PlayerIO playerIO = PlayerIO.Instance;
            List<EquipedCreeture> creatures = playerIO.EquipedCreetures;
            CombatPlayer humanPlayer = new CombatPlayer(creatures);
            CombatPlayer aiPlayer = new CombatPlayer(level.enemyCreatures);
            combatLevelController.init(humanPlayer,aiPlayer,level);
            inputController.LevelController = combatLevelController;
        }
    }
}

