using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels.Combat {
    public interface ICombatAction : IDisplayable
    {
        public string getTitle();
        public string getDescription();
    }

    public interface IManaCombatAction {
        public int getManaCost();
    }
}

