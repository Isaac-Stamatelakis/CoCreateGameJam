using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Player;
using System.Linq;

namespace LevelModule {
    public class OverWorldController : MonoBehaviour
    {
        protected List<WorldNode> nodes;
        protected Transform playerTransform;
        protected PlayerIO playerIO;
        protected Transform lineContainer;
        protected WorldNode currentNode;
        protected List<WorldNode> move;
        protected Dev dev;
        protected Transform levelContainer;
        // Start is called before the first frame update
        void Start()
        {
            move = new List<WorldNode>();
            playerTransform = GameObject.Find("Player").transform;
            playerIO = playerTransform.GetComponent<PlayerIO>();
            lineContainer = transform.Find("Lines");
            levelContainer = transform.Find("Levels");
            dev = playerTransform.GetComponent<Dev>();
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
        }

        void FixedUpdate() {
            handleMove();
        }

        private void handleMove() {
            if (move.Count == 0) {
                return;
            }
            WorldNode node = move[0];
            float dist = Vector2.Distance(node.transform.position,playerTransform.position);
            if (dist < 0.01f) {
                move.RemoveAt(0);
                currentNode = node;
                return;
            }
            playerTransform.position = Vector2.MoveTowards(playerTransform.position,node.transform.position,0.15f);
        }

        private void raycastMove(Vector2 mousePosition) {
            RaycastHit2D hit = Physics2D.Raycast(mousePosition,Vector2.zero,Mathf.Infinity,1 << LayerMask.NameToLayer("WorldNode"));
            if (hit.collider != null) {
                WorldNode worldNode = hit.collider.gameObject.GetComponent<WorldNode>();
                move = getPath(currentNode,worldNode);
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

                foreach (WorldNode nextTile in currentTile.AllConnections)
                {
                    if (!cameFrom.ContainsKey(nextTile))
                    {
                        if (!dev.discoverAll && !playerIO.hasDiscoveredTile(nextTile.name)) {
                            continue;
                        }
                        queue.Enqueue(nextTile);
                        cameFrom[nextTile] = currentTile;

                        if (nextTile == endTile)
                        {
                            return ReconstructPath(cameFrom, startTile, endTile);
                        }
                    }
                }
            }
            return new List<WorldNode>(); // No path found
        }

        private List<WorldNode> ReconstructPath(Dictionary<WorldNode, WorldNode> cameFrom, WorldNode startTile, WorldNode endTile)
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
            
            string tileName = playerIO.CurrentTile;
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
                foreach (WorldNode connection in node.connections) {
                    if (connection == null || node == null) {
                        continue;
                    }
                    if (!dev.discoverAll && (!playerIO.hasDiscoveredTile(node.name) || !playerIO.hasDiscoveredTile(connection.name))) {
                        continue;
                    }
                    // Prevent duplicate lines
                    if (connection.connections.Contains(node)) {
                        connection.connections.Remove(node);
                    }
                    GameObject line = new GameObject();

                    line.name = node.name + " To " + connection.name;
                    
                    line.transform.SetParent(lineContainer);
                    LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
                    Material defaultMaterial = Resources.Load<Material>("Materials/Default.mat");
                    Shader shader = lineRenderer.GetComponent<Shader>();
                    //shader = defaultMaterial;
                    //lineRenderer.material = defaultMaterial;
                    lineRenderer.materials[0] = defaultMaterial;
                    lineRenderer.startWidth = 0.25f;
                    lineRenderer.endWidth = 0.25f;
                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, node.transform.position);
                    lineRenderer.SetPosition(1, connection.transform.position);
                    
                }
            }
            // Make graph doubly connected again
            foreach (WorldNode node in nodes) {
                foreach (WorldNode connection in node.connections) {
                    if (connection == null) {
                        continue;
                    }
                    if (!connection.AllConnections.Contains(node)) {
                        connection.AllConnections.Add(node);      
                    }
                    if (!node.AllConnections.Contains(connection)) {
                        node.AllConnections.Add(connection);      
                    }
                }
            }
            
        }


    }
}

