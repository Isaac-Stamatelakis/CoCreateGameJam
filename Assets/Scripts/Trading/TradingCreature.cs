using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;

namespace Trading {
    public class TradingCreature
    {
        private Creature creature;

        public TradingCreature(Creature creature)
        {
            this.creature = creature;
        }

        public Creature Creature { get => creature; }
    }

}
