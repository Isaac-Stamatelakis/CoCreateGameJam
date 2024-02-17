using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class PlayerIO : MonoBehaviour
    {
        private SPlayerData data;
        public string CurrentTile {get => data.currentTiles; set => data.currentTiles = value;}
        public List<string> EquipmentIds {get => data.equipmentIds; set => data.equipmentIds = value;}
        public bool hasDiscoveredTile(string tileName) {
            return data.discoveredTiles.Contains(tileName);
        }
        // Start is called before the first frame update
        void Awake()
        {
            data = new SPlayerData(
                "Level0", 
                new HashSet<string>{"Level0","Level1"},
                new List<string>{"sword1"}
            );
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private class SPlayerData {
            public SPlayerData(string currentTileIndex, HashSet<string> discoveredTiles, List<string> equipmentIds) {
                this.currentTiles = currentTileIndex;
                this.discoveredTiles = discoveredTiles;
                this.equipmentIds = equipmentIds;
            }
            public string currentTiles; 
            public HashSet<string> discoveredTiles;
            public List<string> equipmentIds;
            public List<string> creatureIds;
        }
    }
}
