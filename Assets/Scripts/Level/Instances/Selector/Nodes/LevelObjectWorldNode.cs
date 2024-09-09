using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels {
    public class LevelObjectWorldNode : WorldNode {
        [SerializeField] private LevelObject level;

        public override ILevel getLevel()
        {
            return level;
        }
    }
}
