using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Levels.Combat;
using Actions;

namespace Actions.MCTS {
    public class SimulatedRandomSelector<T> : CreatureSelector<T> where T : ICombatCreature{
        private int targets;

        public SimulatedRandomSelector(List<T> creatures, int targets) : base(creatures)
        {
            this.targets = targets;
        }

        public float getActionModification() {
            return (float) targets/creatures.Count;
        }

    }
}

