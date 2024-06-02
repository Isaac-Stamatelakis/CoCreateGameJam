using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels.Combat {
    public class SingleTypeCreatureSelector : IMoveCreatureSelector, ISingleTypeCreatureSelector 
    {
        private List<CreatureCombatObject> selected = new List<CreatureCombatObject>();
        public void AddCreature(CreatureCombatObject creatureCombatObject) {
            selected.Add(creatureCombatObject);
        }
        public List<CreatureCombatObject> GetCreatures()
        {
            return selected;
        }
    }
}

