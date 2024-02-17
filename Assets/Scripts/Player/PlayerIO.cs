using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class PlayerIO : MonoBehaviour
    {
        private SPlayerData data;
        public string CurrentTile {get => data.currentTiles; set => data.currentTiles = value;}
        public bool hasDiscoveredTile(string tileName) {
            return data.discoveredTiles.Contains(tileName);
        }
        // Start is called before the first frame update
        void Awake()
        {
            data = new SPlayerData("Level0", new HashSet<string>{"Level0","Level1"});
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private class SPlayerData {
            public SPlayerData(string currentTileIndex, HashSet<string> discoveredTiles) {
                this.currentTiles = currentTileIndex;
                this.discoveredTiles = discoveredTiles;
            }
            public string currentTiles; 
            public HashSet<string> discoveredTiles;
        }
    }
}
