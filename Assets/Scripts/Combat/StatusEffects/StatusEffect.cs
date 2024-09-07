using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels.Combat {
    public abstract class StatusEffect
    {
        public abstract string getName();
    }
    public interface IStatChangeEffect {
        public float modifyStat(float value, CreatureStat creatureStat);
    }
}

