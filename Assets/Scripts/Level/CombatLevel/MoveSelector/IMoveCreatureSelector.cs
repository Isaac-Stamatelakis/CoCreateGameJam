using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;

namespace Levels.Combat {
    public interface IMoveCreatureSelector 
    {
        public List<CreatureCombatObject> GetCreatures();
    }

    public interface ISingleTypeCreatureSelector {
        public void AddCreature(CreatureCombatObject creatureCombatObject);
    }

}
