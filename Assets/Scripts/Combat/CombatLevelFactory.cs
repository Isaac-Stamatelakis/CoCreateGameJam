using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using System.Linq;
using Items;
using Actions.Script;

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

        public static string getEnemyTurnText(string creatureName, ScriptedAction scriptedAction) {
            return $"{creatureName} uses {scriptedAction.name}!";
            //int ran = Random.Range(0,enemyTurnTexts.Count);
            //return enemyTurnTexts[ran].Replace("{name}",creatureName);
        }
    }
}

