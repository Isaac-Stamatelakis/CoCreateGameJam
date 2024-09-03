using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Levels {
    
    public abstract class Level : ScriptableObject
    {
        public abstract string getSceneName();
    }
}

