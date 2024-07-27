using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using System.Linq;
using Items;

namespace Levels.Combat {
    public static class CombatLevelFactory
    {
        public static List<CreatureInCombat> generateTurns(List<CombatPlayer> players) {
            List<CreatureInCombat> creatures = new List<CreatureInCombat>();
            foreach (CombatPlayer player in players) {
                creatures.AddRange(player.Creatures);
            }
            creatures = creatures.OrderBy(creature => creature.EquipedCreeture.getStat(CreatureStat.Speed)).ToList();
            return creatures;
        }
    }
}

