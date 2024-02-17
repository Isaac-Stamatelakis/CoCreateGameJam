using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using LevelModule.Tiles;
using Player;

namespace LevelModule {
    public class OverWorldController : MonoBehaviour
    {
        protected Tilemap mTilemap;
        protected TilemapRenderer mTilemapRenderer;
        protected Dictionary<LevelTile, Vector3Int> levelTilePositions;
        protected Transform playerTransform;
        protected PlayerIO playerIO;
        protected Transform lineContainer;
        protected LevelTile currentPlayerTile;
        protected List<LevelTile> move;
        // Start is called before the first frame update
        void Start()
        {
            mTilemap = GetComponent<Tilemap>();
            mTilemapRenderer = GetComponent<TilemapRenderer>();
            move = new List<LevelTile>();
            playerTransform = GameObject.Find("Player").transform;
            playerIO = playerTransform.GetComponent<PlayerIO>();
            lineContainer = transform.Find("Lines");
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
            LevelTile levelTile = move[0];
            Vector2 levelTileWorldPosition = mTilemap.GetCellCenterWorld(levelTilePositions[levelTile]);
            float dist = Vector2.Distance(levelTileWorldPosition,playerTransform.position);
            if (dist < 0.01f) {
                move.RemoveAt(0);
                currentPlayerTile = levelTile;
                return;
            }
            playerTransform.position = Vector2.MoveTowards(playerTransform.position,levelTileWorldPosition,0.1f);
        }

        private void raycastMove(Vector2 mousePosition) {
            RaycastHit2D hit = Physics2D.Raycast(mousePosition,Vector2.zero,Mathf.Infinity,1 << LayerMask.NameToLayer("LevelTile"));
            if (hit.collider != null) {
                LevelTile levelTile = findClosestLevelTileHit(mousePosition);
                move = getPath(currentPlayerTile,levelTile);
            }
        }

        private List<LevelTile> getPath(LevelTile startTile, LevelTile endTile) {
            if (startTile == endTile)
            {
                return new List<LevelTile> { startTile };
            }

            Queue<LevelTile> queue = new Queue<LevelTile>();
            Dictionary<LevelTile, LevelTile> cameFrom = new Dictionary<LevelTile, LevelTile>();
            queue.Enqueue(startTile);
            cameFrom[startTile] = null;

            while (queue.Count > 0)
            {
                LevelTile currentTile = queue.Dequeue();

                foreach (LevelTile nextTile in currentTile.FullConnections)
                {
                    if (!cameFrom.ContainsKey(nextTile))
                    {
                        if (!playerIO.hasDiscoveredTile(nextTile.name)) {
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
            return new List<LevelTile>(); // No path found
        }

        private List<LevelTile> ReconstructPath(Dictionary<LevelTile, LevelTile> cameFrom, LevelTile startTile, LevelTile endTile)
        {
            List<LevelTile> path = new List<LevelTile>();
            LevelTile currentTile = endTile;

            while (currentTile != startTile)
            {
                path.Add(currentTile);
                currentTile = cameFrom[currentTile];
            }
            path.Reverse();
            return path;
        }

        private LevelTile findClosestLevelTileHit(Vector2 mousePosition) {
            Vector3Int cellPosition = mTilemap.WorldToCell(mousePosition);
            for (int x = -1; x <= 1; x ++) {
                for (int y = -1; y <= 1; y++) {
                    TileBase tileBase = mTilemap.GetTile(cellPosition+ new Vector3Int(x,y,0));
                    if (tileBase != null && tileBase is LevelTile) {
                        return (LevelTile)tileBase;
                    }
                }
            }
            return null;    
        }
        private void initPlayer() {
            
            string tileName = playerIO.CurrentTile;
            currentPlayerTile = null;
            foreach (LevelTile levelTile in levelTilePositions.Keys) {
                if (levelTile.name.Equals(tileName)) {
                    currentPlayerTile = levelTile;
                    break;
                }
            }
            playerTransform.position = mTilemap.GetCellCenterWorld(levelTilePositions[currentPlayerTile]);
            
        }
        private void getLevelTiles() {
            levelTilePositions = new Dictionary<LevelTile, Vector3Int>();
            BoundsInt bounds = mTilemap.cellBounds;
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cellPosition = new Vector3Int(x, y, 0);
                    TileBase tile = mTilemap.GetTile(cellPosition);
                    if (tile != null)
                    {
                        if (tile is not LevelTile) {
                            continue;
                        }
                        LevelTile levelTile = (LevelTile) tile;
                        levelTilePositions[levelTile] = cellPosition;
                    }
                }
            }
            Debug.Log(levelTilePositions.Count + " Level Tiles Loaded");
        }
        private void drawLines() {
            foreach (KeyValuePair<LevelTile,Vector3Int> levelTilePosition in levelTilePositions) {
                foreach (LevelTile connection in levelTilePosition.Key.connections) {
                    if (!levelTilePositions.ContainsKey(connection)) {
                        Debug.LogError("Connection " + connection.name + " for " + levelTilePosition.Key.name + " not in dictionary");
                        continue;
                    }
                    if (!playerIO.hasDiscoveredTile(levelTilePosition.Key.name) || !playerIO.hasDiscoveredTile(connection.name)) {
                        continue;
                    }
                    // Prevent duplicate lines
                    if (connection.connections.Contains(levelTilePosition.Key)) {
                        connection.connections.Remove(levelTilePosition.Key);
                    }
                    Vector3 levelTilePos = mTilemap.GetCellCenterWorld(levelTilePosition.Value);
                    Vector3 connectionPos = mTilemap.GetCellCenterWorld(levelTilePositions[connection]);
                    GameObject line = new GameObject();
                    line.name = levelTilePosition.Key.name + " To " + connection.name;
                    
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
                    lineRenderer.SetPosition(0, levelTilePos);
                    lineRenderer.SetPosition(1, connectionPos);
                    
                }
            }
            // Make graph doubly connected again
            
            foreach (KeyValuePair<LevelTile,Vector3Int> levelTilePosition in levelTilePositions) {
                foreach (LevelTile connection in levelTilePosition.Key.connections) {
                    // Prevent duplicate lines
                    if (connection.FullConnections.Contains(levelTilePosition.Key)) {
                        continue;
                    }
                    connection.FullConnections.Add(levelTilePosition.Key);      
                    if (levelTilePosition.Key.FullConnections.Contains(connection)) {
                        continue;
                    }     
                    levelTilePosition.Key.FullConnections.Add(connection);
                }
            }
            
        }


    }
}

