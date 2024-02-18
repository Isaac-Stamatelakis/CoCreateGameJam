using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LevelModule {
    
    public abstract class Level : ScriptableObject
    {
        public GameObject levelPrefab;
        public abstract void load();
    }
}

