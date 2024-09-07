using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;

namespace Actions.MCTS {
    public interface ISimulatedSelector {
        public CreatureInCombat getTarget(int iteration);
        public int maxIterations();
    }
    public class SimulatedSelector : ISimulatedSelector
    {
        protected List<CreatureInCombat> creatures;
        public CreatureInCombat getTarget(int iteration)
        {
            return creatures[iteration];
        }

        public int maxIterations()
        {
            return creatures.Count;
        }
    }

    public class SimulatedRandomSelector : SimulatedSelector {
        private int targets;
        public float getActionModification() {
            return (float) targets/creatures.Count;
        }

    }
}

