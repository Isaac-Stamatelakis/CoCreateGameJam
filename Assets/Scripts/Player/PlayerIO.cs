using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using CreatureModule;
using InventoryModule;

namespace Player {
    public class PlayerIO : MonoBehaviour
    {
        private SPlayerData data;
        private List<EquipedCreeture> equipedCreetures;
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
                new List<string>{"sword1","sword2","sword3"},
                new List<SCreatureData>{
                    new SCreatureData("shiny_jeez", new List<string>{}),
                    new SCreatureData("shiny_mike", new List<string>{}),
                    new SCreatureData("crog", new List<string>{})
                }
            );
        }

        public List<Creeture> GetCreetures() {
            CreetureRegistry creetureRegistry = CreetureRegistry.getInstance();
            List<Creeture> returnVal = new List<Creeture>();
            foreach (SCreatureData data in data.creatureData) {
                Creeture creeture = GameObject.Instantiate(creetureRegistry.getEquipment(data.id));

                if (creeture == null) {
                    continue;
                }
                returnVal.Add(creeture);
            }
            return returnVal;
        }

        public List<EquipedCreeture> GetEquipedCreetures() {
            if (equipedCreetures == null) {
                equipedCreetures = new List<EquipedCreeture>();
                CreetureRegistry creetureRegistry = CreetureRegistry.getInstance();
                EquipmentRegistry equipmentRegistry = EquipmentRegistry.getInstance();
                foreach (SCreatureData sCreatureData in data.creatureData) {
                    List<Equipment> equipments = new List<Equipment>();
                    Creeture creeture = creetureRegistry.getEquipment(sCreatureData.id);
                    if (creeture == null) {
                        continue;
                    }
                    foreach (string equipmentID in sCreatureData.equipmentIDs) {
                        Equipment equipment = equipmentRegistry.getEquipment(equipmentID);
                        if (equipment == null) {
                            continue;
                        }
                        equipments.Add(equipment);
                    }
                    equipedCreetures.Add(new EquipedCreeture(creeture,equipments));
                }
            }
            return equipedCreetures;
        }

        public List<Equipment> GetEquipment() {
            EquipmentRegistry registry = EquipmentRegistry.getInstance();
            List<Equipment> returnVal = new List<Equipment>();
            foreach (string id in data.equipmentIds) {
                Equipment equipment = registry.getEquipment(id);
                if (equipment == null) {
                    continue;
                }
                returnVal.Add(equipment);
            }
            return returnVal;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private class SPlayerData {
            public SPlayerData(string currentTileIndex, HashSet<string> discoveredTiles, List<string> equipmentIds, List<SCreatureData> creatureData) {
                this.currentTiles = currentTileIndex;
                this.discoveredTiles = discoveredTiles;
                this.equipmentIds = equipmentIds;
                this.creatureData = creatureData;
            }
            public string currentTiles; 
            public HashSet<string> discoveredTiles;
            public List<string> equipmentIds;
            public List<SCreatureData> creatureData;
        }

        private class SCreatureData {
            public string id;
            public List<string> equipmentIDs;
            public SCreatureData(string id, List<string> equipmentIDs) {
                this.id = id;
                this.equipmentIDs = equipmentIDs;
            }
        }
    }
}
