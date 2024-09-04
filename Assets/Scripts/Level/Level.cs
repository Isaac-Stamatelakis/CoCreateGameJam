using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Levels {


    public interface ILevel {
        public string getSceneName();
    }
    public abstract class LevelObject : ScriptableObject, ILevel
    {
        public abstract string getSceneName();
    }

    public abstract class Level : ILevel
    {
        public abstract string getSceneName();
    }
}

