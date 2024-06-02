using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures {
    public static class CreatureUtils
    {
        public static int getExperienceToLevel(int level) {
            return level * level * 100;
        }
    }
}

