using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelModule.Tiles {
    [CreateAssetMenu(fileName = "New Level Tile", menuName = "Level/Tile")]
    public class LevelTile : Tile
    {
        [Header("The level which this tile contains")]
        public Level level;
        public List<LevelTile> connections;
        private List<LevelTile> fullConnections;
        public List<LevelTile> FullConnections { get => fullConnections; set => fullConnections = value; }
    }
}

