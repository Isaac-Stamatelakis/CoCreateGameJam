using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelModule {
    [System.Serializable]

    public class WorldNode : MonoBehaviour
    {
        public Level level;
        public List<WorldNode> connections = new List<WorldNode>();
        private List<WorldNode> allConnections = new List<WorldNode>();

        public List<WorldNode> AllConnections { get => allConnections; set => allConnections = value; }

        // Start is called before the first frame update
        void Start()
        {
               
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }

}
