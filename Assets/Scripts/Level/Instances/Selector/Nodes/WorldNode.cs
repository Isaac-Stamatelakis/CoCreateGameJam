using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels {
    public interface IWorldNode {
        public ILevel getLevel();
        public bool getRequireAllConnections();
    }

    public abstract class WorldNode : MonoBehaviour, IWorldNode  {
        [SerializeField] private List<WorldNode> connections;
        [SerializeField] private bool requireAllConnections = true;
        public List<WorldNode> Connections {get => connections;}
        public HashSet<WorldNode> LineConnections { get => lineConnections;}
        private HashSet<WorldNode> lineConnections = new HashSet<WorldNode>();
        public void addLineConnection(WorldNode worldNode) {
            LineConnections.Add(worldNode);
        }
        public bool containsConnection(WorldNode worldNode) {
            return LineConnections.Contains(worldNode);
        }
        

        public abstract ILevel getLevel();

        public bool getRequireAllConnections()
        {
            return requireAllConnections;
        }
    }


}
