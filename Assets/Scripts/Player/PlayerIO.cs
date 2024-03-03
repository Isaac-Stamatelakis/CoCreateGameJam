using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using CreatureModule;
using InventoryModule;
using LootBoxModule;
using WorldCreationModule;
using System.IO;

namespace Player {
    public class PlayerIO : MonoBehaviour
    {
        private SPlayerData sData;
        public List<EquipedCreeture> EquipedCreetures {get => playerData.creetures; set => playerData.creetures = value;}
        public List<Equipment> Equipment {get => playerData.equipment; set => playerData.equipment = value;}
        public List<LootboxCount> LootBoxes {get => playerData.lootboxes; set => playerData.lootboxes = value;}
        private PlayerData playerData;
        public string CurrentTile {get => sData.currentTile; set => sData.currentTile = value;}
        public bool hasDiscoveredTile(string tileName) {
            return sData.discoveredTiles.Contains(tileName);
        }
        // Start is called before the first frame update
        
        public void Awake() {
            string path = WorldCreation.getPlayerDataPath(Global.WorldName);
            string data = File.ReadAllText(path);
            deseralize(data);
        }

        public void OnDestroy() {
            string data = seralize();
            string path = WorldCreation.getPlayerDataPath(Global.WorldName);
            File.WriteAllText(path,data);
        }

        public void deseralize(string data) {
            Debug.Log(data);
            SPlayerData sPlayerData = JsonConvert.DeserializeObject<SPlayerData>(data);
            List<Equipment> equipment = DeseralizeEquipment(sPlayerData.equipmentIds);
            List<EquipedCreeture> equipedCreetures = DeseralizeCreetures(sPlayerData.creatureData);
            List<LootboxCount> lootboxCounts = deseralizeLootboxes(sPlayerData.lootboxData);
            playerData = new PlayerData(
                currentTile: sPlayerData.currentTile,
                discoveredTiles: sPlayerData.discoveredTiles,
                creetures: equipedCreetures,
                equipment: equipment,
                lootboxCounts: lootboxCounts
            );
        }

        public string seralize() {
            List<string> equipmentIds = seralizeEquipment(playerData.equipment);
            List<SLootboxData> lootboxData = new List<SLootboxData>();
            foreach (LootboxCount lootboxCount in playerData.lootboxes) {
                lootboxData.Add(new SLootboxData(lootboxCount.lootBox.id,lootboxCount.count));
            }
            List<SCreatureData> creatureData = new List<SCreatureData>();
            foreach (EquipedCreeture equipedCreeture in playerData.creetures) {
                creatureData.Add(new SCreatureData(equipedCreeture.creeture.id,seralizeEquipment(equipedCreeture.equipment)));
            }
            SPlayerData sPlayerData = new SPlayerData(
                currentTileIndex: playerData.currentTile,
                discoveredTiles: playerData.discoveredTiles,
                equipmentIds: equipmentIds,
                creatureData: creatureData,
                lootboxData: lootboxData
            );
            return JsonConvert.SerializeObject(sPlayerData);
        }

        private List<string> seralizeEquipment(List<Equipment> equipmentList) {
            List<string> equipmentIds = new List<string>();
            foreach (Equipment equipment in equipmentList) {
                equipmentIds.Add(equipment.id);
            }
            return equipmentIds;
        }

        public List<Creeture> GetCreetures() {
            CreetureRegistry creetureRegistry = CreetureRegistry.getInstance();
            List<Creeture> returnVal = new List<Creeture>();
            foreach (SCreatureData data in sData.creatureData) {
                Creeture creeture = GameObject.Instantiate(creetureRegistry.getEquipment(data.id));

                if (creeture == null) {
                    continue;
                }
                returnVal.Add(creeture);
            }
            return returnVal;
        }

        private List<EquipedCreeture> DeseralizeCreetures(List<SCreatureData> sCreatureData) {
            List<EquipedCreeture> equipedCreetures = new List<EquipedCreeture>();
            CreetureRegistry creetureRegistry = CreetureRegistry.getInstance();
            EquipmentRegistry equipmentRegistry = EquipmentRegistry.getInstance();
            foreach (SCreatureData data in sCreatureData) {
                List<Equipment> equipments = new List<Equipment>();
                Creeture creeture = creetureRegistry.getEquipment(data.id);
                if (creeture == null) {
                    continue;
                }
                foreach (string equipmentID in data.equipmentIDs) {
                    Equipment equipment = equipmentRegistry.getEquipment(equipmentID);
                    if (equipment == null) {
                        continue;
                    }
                    equipments.Add(equipment);
                }
                equipedCreetures.Add(new EquipedCreeture(creeture,equipments));
                
            }
            return equipedCreetures;
        }

        public static string initPlayerData() {
            SPlayerData sPlayerData = new SPlayerData(
                null,
                new List<string>(),
                new List<string>(),
                new List<SCreatureData>(),
                new List<SLootboxData>()
            );
            return JsonConvert.SerializeObject(sPlayerData);
        }

        private List<LootboxCount> deseralizeLootboxes(List<SLootboxData> lootboxData) {
            List<LootboxCount> lootboxCounts = new List<LootboxCount>();
            LootBoxRegistry lootBoxRegistry = LootBoxRegistry.getInstance();
            foreach (SLootboxData data in lootboxData) {
                LootBox lootBox = lootBoxRegistry.getLootbox(data.id);
                lootboxCounts.Add(new LootboxCount(lootBox,data.count));
            }
            return lootboxCounts;
        }

        public List<Equipment> DeseralizeEquipment(List<string> ids) {
            EquipmentRegistry registry = EquipmentRegistry.getInstance();
            List<Equipment> returnVal = new List<Equipment>();
            foreach (string id in ids) {
                Equipment equipment = registry.getEquipment(id);
                if (equipment == null) {
                    continue;
                }
                returnVal.Add(equipment);
            }
            return returnVal;
        }

        public List<LootboxCount> getLootboxes() {
            return null;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private class SPlayerData {
            public SPlayerData(
                string currentTileIndex, 
                List<string> discoveredTiles, 
                List<string> equipmentIds, 
                List<SCreatureData> creatureData,
                List<SLootboxData> lootboxData
            ) {
                this.currentTile = currentTileIndex;
                this.discoveredTiles = discoveredTiles;
                this.equipmentIds = equipmentIds;
                this.creatureData = creatureData;
                this.lootboxData = lootboxData;
            }
            public string currentTile; 
            public List<string> discoveredTiles;
            public List<string> equipmentIds;
            public List<SCreatureData> creatureData;
            public List<SLootboxData> lootboxData;
        }

        private class PlayerData {
            public PlayerData(List<string> discoveredTiles, string currentTile, List<EquipedCreeture> creetures, List<Equipment> equipment, List<LootboxCount> lootboxCounts) {
                this.discoveredTiles = discoveredTiles;
                this.currentTile = currentTile;
                this.creetures = creetures;
                this.equipment = equipment;
                this.lootboxes = lootboxCounts;
            }
            public List<string> discoveredTiles;
            public string currentTile;
            public List<EquipedCreeture> creetures;
            public List<Equipment> equipment;
            public List<LootboxCount> lootboxes;
        }

        private class SCreatureData {
            public string id;
            public List<string> equipmentIDs;
            public SCreatureData(string id, List<string> equipmentIDs) {
                this.id = id;
                this.equipmentIDs = equipmentIDs;
            }
        }

        private class SLootboxData {
            public SLootboxData(string id, int count) {
                this.id = id;
                this.count = count;
            }
            public string id;
            public int count;
        }
    }
}
