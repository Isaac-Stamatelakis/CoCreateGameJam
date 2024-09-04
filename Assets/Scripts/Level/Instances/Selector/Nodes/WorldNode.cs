using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels {
    public interface IWorldNode {
        public ILevel getLevel();
    }

    public abstract class WorldNode : MonoBehaviour, IWorldNode  {
        [SerializeField] private List<WorldNode> connections;
        public List<WorldNode> Connections {get => connections;}
        private HashSet<WorldNode> lineConnections = new HashSet<WorldNode>();
        public void addLineConnection(WorldNode worldNode) {
            lineConnections.Add(worldNode);
        }
        public bool containsConnection(WorldNode worldNode) {
            return lineConnections.Contains(worldNode);
        }

        public abstract ILevel getLevel();
    }


}
