using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Levels {
    
    public abstract class Level : ScriptableObject
    {
        public GameObject levelPrefab;
        public abstract string getSceneName();
        public abstract void load();
    }
}

