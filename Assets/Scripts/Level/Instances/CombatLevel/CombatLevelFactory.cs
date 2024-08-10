using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using System.Linq;
using Items;

namespace Levels.Combat {
    public static class CombatLevelUtils
    {
        private static readonly List<string> enemyTurnTexts = new List<string>{
            "Enemy {name} Prepares to Strike!",
            "{name} is Making a Move!",
            "Beware! {name} is Attacking!",
            "{name} Launches an Attack!",
            "Enemy {name} Strikes with Fury!"
        };
        public static List<CreatureInCombat> generateTurns(List<CombatPlayer> players) {
            List<CreatureInCombat> creatures = new List<CreatureInCombat>();
            foreach (CombatPlayer player in players) {
                creatures.AddRange(player.Creatures);
            }
            creatures = creatures.OrderBy(creature => creature.EquipedCreeture.getStat(CreatureStat.Speed)).ToList();
            return creatures;
        }

        public static string getEnemyTurnText(string creatureName) {
            int ran = Random.Range(0,enemyTurnTexts.Count);
            return enemyTurnTexts[ran].Replace("{name}",creatureName);
        }
    }
}

