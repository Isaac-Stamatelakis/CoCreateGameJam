using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Creatures;
using Items;
using LootBoxes;
using WorldCreationModule;
using System.IO;
using Trading;

namespace Player {
    public class PlayerIO : MonoBehaviour
    {
        private static PlayerIO instance;
        private List<EquipedCreeture> combatCreatures;
        public List<EquipedCreeture> EquipedCreetures {get => playerData.creetures; set => playerData.creetures = value;}
        public List<Equipment> Equipment {get => playerData.equipment; set => playerData.equipment = value;}
        public List<LootboxCount> LootBoxes {get => playerData.lootboxes; set => playerData.lootboxes = value;}
        public List<ItemSlot> CraftingItems {get => playerData.craftingItems; set => playerData.craftingItems = value;}
        private PlayerData playerData;
        public string CurrentTile {get => playerData.currentTile; set => playerData.currentTile = value;}
        public static PlayerIO Instance { get => instance;}
        public List<CurrencyCount> Currencies {get=>playerData.currencyCounts;}

        public bool hasDiscoveredTile(string tileName) {
            return playerData.discoveredTiles.Contains(tileName);
        }
        public void Awake() {
            instance = this;
            string path = WorldCreation.getPlayerDataPath(Global.WorldName);
            playerData = File.Exists(path) ? deseralize(File.ReadAllText(path)) : initPlayerData();
        }

        public void OnDestroy() {
            string data = seralize();
            string path = WorldCreation.getPlayerDataPath(Global.WorldName);
            File.WriteAllText(path,data);
        }

        public void give(LootableCount lootableCount) {
            Lootable lootable = lootableCount.lootable;
            if (lootable is LootBox lootBox) {
                LootboxCount lootboxCount = new LootboxCount(lootBox,(int)lootableCount.count);
                LootableCountUtils.giveCountable<LootboxCount>(lootboxCount,LootBoxes);
            } else if (lootable is Equipment equipment) {
                playerData.equipment.Add(equipment);
            } else if (lootable is Creature creeture) {
                playerData.creetures.Add(new EquipedCreeture(creeture,new List<Equipment>()));
            } else if (lootable is Currency currency) {
                CurrencyCount currencyCount = new CurrencyCount(currency,lootableCount.count);
                LootableCountUtils.giveCountable<CurrencyCount>(currencyCount,Currencies);
            } else if (lootable is CraftingItem itemSlot) {

            } else {
                throw new System.Exception($"Did not cover case for lootable {lootable.GetType()}");
            }
        }

        
        private PlayerData deseralize(string data) {
            try {
                SPlayerData sPlayerData = JsonConvert.DeserializeObject<SPlayerData>(data);
                List<Equipment> equipment = DeseralizeEquipment(sPlayerData.equipmentIds);
                EquipCreatureSerializationFactory equipCreatureSerializationFactory = new EquipCreatureSerializationFactory();
                List<EquipedCreeture> equipedCreetures = equipCreatureSerializationFactory.deserializeList(sPlayerData.creatureData);
                List<LootboxCount> lootboxCounts = deseralizeLootboxes(sPlayerData.lootboxData);
                List<CurrencyCount> currencyCounts = new CurrencyCountSerializer().deserializeList(sPlayerData.currencyCountData);
                List<ItemSlot> craftingItems = new ItemSlotSerializationFactory().deserializeList(sPlayerData.craftingItemData);
                return new PlayerData(
                    currentTile: sPlayerData.currentTile,
                    discoveredTiles: sPlayerData.discoveredTiles,
                    creetures: equipedCreetures,
                    equipment: equipment,
                    lootboxCounts: lootboxCounts,
                    currencyCounts: currencyCounts,
                    craftingItems: craftingItems
                );
            } catch (JsonSerializationException e) {
                Debug.LogError($"Error during player deseralization {e}");
                return initPlayerData();
            }
            
            
        }

        public string seralize() {
            List<string> equipmentIds = EquipmentFactory.serialize(playerData.equipment);
            List<SLootboxData> lootboxData = new List<SLootboxData>();
            foreach (LootboxCount lootboxCount in playerData.lootboxes) {
                lootboxData.Add(new SLootboxData(lootboxCount.lootBox.getId(),lootboxCount.count));
            }
            string creatureData = new EquipCreatureSerializationFactory().serialize(playerData.creetures);
            string currencyData = new CurrencyCountSerializer().serialize(playerData.currencyCounts);
            string craftingItemData = new ItemSlotSerializationFactory().serialize(playerData.craftingItems);
            SPlayerData sPlayerData = new SPlayerData(
                currentTileIndex: playerData.currentTile,
                discoveredTiles: playerData.discoveredTiles,
                equipmentIds: equipmentIds,
                creatureData: creatureData,
                lootboxData: lootboxData,
                currencyData: currencyData,
                craftingItems: craftingItemData
            );
            return JsonConvert.SerializeObject(sPlayerData);
        }

        private static PlayerData initPlayerData() {
            LootBox cardboardBox = LootBoxRegistry.getInstance().getLootbox("cardboard_box");
            LootboxCount startingBox = new LootboxCount(
                cardboardBox,
                9999
            );
            return new PlayerData(
                new List<string>(),
                null,
                new List<EquipedCreeture>(),
                new List<Equipment>(),
                new List<LootboxCount>{startingBox},
                new List<CurrencyCount>(),
                new List<ItemSlot>()
            );
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
            LootableRegistry registry = LootableRegistry.getInstance();
            List<Equipment> returnVal = new List<Equipment>();
            foreach (string id in ids) {
                Equipment equipment = registry.getLootable<Equipment>(id);
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

        [System.Serializable]
        private class SPlayerData {
            public SPlayerData(
                string currentTileIndex, 
                List<string> discoveredTiles, 
                List<string> equipmentIds, 
                string creatureData,
                List<SLootboxData> lootboxData,
                string currencyData,
                string craftingItems
            ) {
                this.currentTile = currentTileIndex;
                this.discoveredTiles = discoveredTiles;
                this.equipmentIds = equipmentIds;
                this.creatureData = creatureData;
                this.lootboxData = lootboxData;
                this.currencyCountData = currencyData;
                this.craftingItemData = craftingItems;
            }
            public string currentTile; 
            public List<string> discoveredTiles;
            public List<string> equipmentIds;
            public string creatureData;
            public List<SLootboxData> lootboxData;
            public string currencyCountData;
            public string craftingItemData;
        }

        private class PlayerData {
            public PlayerData(
                List<string> discoveredTiles, 
                string currentTile, 
                List<EquipedCreeture> creetures, 
                List<Equipment> equipment, 
                List<LootboxCount> lootboxCounts,
                List<CurrencyCount> currencyCounts,
                List<ItemSlot> craftingItems
                ) {
                this.discoveredTiles = discoveredTiles;
                this.currentTile = currentTile;
                this.creetures = creetures;
                this.equipment = equipment;
                this.lootboxes = lootboxCounts;
                this.currencyCounts = currencyCounts;
                this.craftingItems = craftingItems;
            }
            public List<string> discoveredTiles;
            public string currentTile;
            public List<EquipedCreeture> creetures;
            public List<Equipment> equipment;
            public List<LootboxCount> lootboxes;
            public List<CurrencyCount> currencyCounts;
            public List<ItemSlot> craftingItems;
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
