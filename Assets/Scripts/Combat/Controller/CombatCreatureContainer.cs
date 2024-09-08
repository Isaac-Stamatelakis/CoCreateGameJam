using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;

namespace Levels.Combat {
    public class CombatCreatureContainer : MonoBehaviour
    {
        private CreatureCombatObject[] creatureObjects;
        public void Awake() {
            creatureObjects = GetComponentsInChildren<CreatureCombatObject>();
        }

        public void displayCreatures(
            List<CreatureInCombat> creatures
        ) {
            int toShow =  Mathf.Min(creatures.Count,Global.MAX_COMBAT_CREATURES);
            List<CreatureCombatObject> combatObjects = new List<CreatureCombatObject>();
            for (int i = 0; i < toShow; i++) {
                creatureObjects[i].display(creatures[i]);
                creatures[i].setCombatObject(creatureObjects[i]);
            }
            // Hide inactive creatures
            for (int i = toShow; i < Global.MAX_COMBAT_CREATURES; i++) {
                GameObject.Destroy(creatureObjects[i].gameObject);
            }
        }
    }
}

