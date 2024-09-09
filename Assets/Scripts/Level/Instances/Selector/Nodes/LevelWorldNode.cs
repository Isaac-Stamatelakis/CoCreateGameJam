using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels {
    public class LevelWorldNode : WorldNode
    {
        private Level level;
        public void setLevel(Level level) {
            this.level = level;
        }
        public override ILevel getLevel()
        {
            return level;
        }
    }
}

