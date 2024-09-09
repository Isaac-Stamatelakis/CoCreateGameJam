using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Player;
using System.Linq;

namespace Levels {
    public class WorldNodeController : MonoBehaviour
    {
        protected List<WorldNode> nodes;
        [SerializeField] private Transform levelContainer;
        [SerializeField] protected Transform lineContainer;
        [SerializeField] private GameObject linePrefab;
        private WorldNode currentNode;
        private List<WorldNode> moveList = new List<WorldNode>();
        [SerializeField] private Dev dev;
        [SerializeField] protected Transform playerTransform;
        private HashSet<string> discoveredNodes;
        // Start is called before the first frame update
        void Start()
        {
            //discoveredTiles = PlayerIO.Instance.
            getLevelTiles();
            drawLines();
            initPlayer();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0)) {
                raycastMove(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
            if (Input.GetKeyDown(KeyCode.Space) && moveList.Count == 0) {
                ILevel level = currentNode.getLevel();
                if (level != null) {
                    LevelManager.changeLevel(level); 
                }
            }
        }

        void FixedUpdate() {
            handleMove();
        }

        private void handleMove() {
            if (moveList.Count == 0) {
                return;
            }
            WorldNode node = moveList[0];
            float dist = Vector2.Distance(node.transform.position,playerTransform.position);
            if (dist < 0.01f) {
                moveList.RemoveAt(0);
                currentNode = node;
                return;
            }
            playerTransform.position = Vector2.MoveTowards(playerTransform.position,node.transform.position,0.15f);
        }

        private void raycastMove(Vector2 mousePosition) {
            RaycastHit2D hit = Physics2D.Raycast(mousePosition,Vector2.zero,Mathf.Infinity,1 << LayerMask.NameToLayer("WorldNode"));
            if (hit.collider != null) {
                WorldNode worldNode = hit.collider.gameObject.GetComponent<WorldNode>();
                moveList = getPath(currentNode,worldNode);
            }
        }

        private List<WorldNode> getPath(WorldNode startTile, WorldNode endTile) {
            if (startTile == endTile)
            {
                return new List<WorldNode> { startTile };
            }

            Queue<WorldNode> queue = new Queue<WorldNode>();
            Dictionary<WorldNode, WorldNode> cameFrom = new Dictionary<WorldNode, WorldNode>();
            queue.Enqueue(startTile);
            cameFrom[startTile] = null;

            while (queue.Count > 0)
            {
                WorldNode currentTile = queue.Dequeue();
                foreach (WorldNode nextTile in currentTile.LineConnections)
                {
                    if (!cameFrom.ContainsKey(nextTile))
                    {
                        if (!dev.discoverAll && !discoveredNodes.Contains(nextTile.name)) {
                            continue;
                        }
                        queue.Enqueue(nextTile);
                        cameFrom[nextTile] = currentTile;

                        if (nextTile == endTile)
                        {
                            return reconstructPath(cameFrom, startTile, endTile);
                        }
                    }
                }
            }
            return new List<WorldNode>(); // No path found
        }

        private List<WorldNode> reconstructPath(Dictionary<WorldNode, WorldNode> cameFrom, WorldNode startTile, WorldNode endTile)
        {
            List<WorldNode> path = new List<WorldNode>();
            WorldNode currentTile = endTile;

            while (currentTile != startTile)
            {
                path.Add(currentTile);
                currentTile = cameFrom[currentTile];
            }
            path.Reverse();
            return path;
        }

        private void initPlayer() {
            // Todo use serialization
            string tileName = "Level0";
            if (tileName == null) {
                tileName = Global.StartSquare;
            }
            currentNode = null;
            foreach (WorldNode node in nodes) {
                if (node.name.Equals(tileName)) {
                    currentNode = node;
                    break;
                }
            }
            playerTransform.position = currentNode.transform.position;
            
        }
        private void getLevelTiles() {
            nodes = levelContainer.GetComponentsInChildren<WorldNode>().ToList();
            Debug.Log(nodes.Count + " Level Tiles Loaded");
        }
        private void drawLines() {
            foreach (WorldNode node in nodes) {
                foreach (WorldNode connection in node.Connections) {
                    if (connection == null || node == null) {
                        continue;
                    }
                    if (!dev.discoverAll && (!discoveredNodes.Contains(node.name) || !discoveredNodes.Contains(connection.name))) {
                        continue;
                    }
                    if (connection.containsConnection(node)) {
                        continue;
                    }
                    node.addLineConnection(connection);
                    connection.addLineConnection(node);
                    LineFactory.create(node,connection,lineContainer,linePrefab);
                }
            }
            Debug.Log(lineContainer.childCount + " Lines Drawn");
        }


    }
}

