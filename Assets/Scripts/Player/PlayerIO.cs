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
using System.Linq;
using System;

namespace Player {
    public class PlayerIO : MonoBehaviour
    {
        private static PlayerIO instance;
        private List<EquipedCreeture> combatCreatures;
        public List<EquipedCreeture> EquipedCreetures {get => playerData.creetures; set => playerData.creetures = value;}
        public List<Equipment> Equipment {get => playerData.equipment; set => playerData.equipment = value;}
        public List<IntItemSlot> LootBoxes {get => playerData.lootboxes; set => playerData.lootboxes = value;}
        public List<IntItemSlot> CraftingItems {get => playerData.craftingItems; set => playerData.craftingItems = value;}
        private PlayerData playerData;
        public string CurrentTile {get => playerData.currentTile; set => playerData.currentTile = value;}
        public static PlayerIO Instance { get => instance;}
        public List<DoubleItemSlot> Currencies {get=>playerData.currencyCounts;}

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
        public void give(List<IntItemSlot> itemSlots) {
            foreach (IntItemSlot itemSlot in itemSlots) {
                give(itemSlot);
            }
        }
        public void give(IntItemSlot itemSlot) {
            if (itemSlot == null || itemSlot.Lootable == null) {
                return;
            }
            Lootable lootable = itemSlot.Lootable;
            if (lootable is Creature creature) {
                playerData.creetures.Add(new EquipedCreeture(creature,new List<Equipment>()));
            } else if (lootable is Equipment equipment) {
                playerData.equipment.Add(equipment);
            } else if (lootable is CraftingItem craftingItem) {
                Debug.Log("Here");
                ItemSlotFactory.insertList<IntItemSlot>(playerData.craftingItems,itemSlot);
                Debug.Log(playerData.craftingItems.Count);
            } else if (lootable is LootBox lootBox) {
                ItemSlotFactory.insertList<IntItemSlot>(playerData.lootboxes,itemSlot);
            } else if (lootable is Currency currency) {
                DoubleItemSlot currencySlot = ItemSlotFactory.fromIntItemSlot(itemSlot);
                ItemSlotFactory.insertList<DoubleItemSlot>(playerData.currencyCounts,currencySlot);
            }
        }

        public void give(DoubleItemSlot doubleItemSlot) {

        }

        public int getAmountOfUniqueItem(string id, TieredItemType tieredItemType, Rarity rarity) {
            switch (tieredItemType) {
                case TieredItemType.Creature:
                    return ItemSlotFactory.getAmountFromList(playerData.creetures.Cast<UniqueItemSlot>().ToList(),id,rarity);
                case TieredItemType.Equipment:
                    return ItemSlotFactory.getAmountFromList(playerData.equipment.Cast<UniqueItemSlot>().ToList(),id,rarity);
            }
            return 0;
            
        }
        public float getAmountOfLootable(ItemSlot itemSlot) {
            /*
            if (itemSlot == null || itemSlot.Lootable == null) {
                return 0;
            }
            if (itemSlot.Lootable is LootBox lootBox) {
                return LootableCountUtils.getCount<LootBox,LootboxCount>(lootBox,playerData.lootboxes);
            } else if (itemSlot.Lootable is Currency currency) {
                return LootableCountUtils.getCount<Currency,CurrencyCount>(currency,playerData.currencyCounts);
            } else if (itemSlot.Lootable is CraftingItem craftingItem) {
                return LootableCountUtils.getCount<CraftingItem,ItemSlot>(craftingItem,playerData.craftingItems);
            }
            return 0;
            */
            return 0;


        }

        public void take(TieredItemType tieredItemType, int index) {
            List<object> elements = getLootableList(tieredItemType);
            if (index < 0 || index >= elements.Count) {
                return;
            }
            elements.RemoveAt(index);
        }

        public void take(ItemSlot itemSlot) {
            if (itemSlot == null || itemSlot.Lootable == null) {
                return;
            }
            Lootable lootable = itemSlot.Lootable;
            if (lootable is Currency currency) {

            }
        }

        private List<object> getLootableList(TieredItemType tieredItemType) {
            switch (tieredItemType) {
                case TieredItemType.Creature:
                    return playerData.creetures.Cast<object>().ToList();
                case TieredItemType.Equipment:
                    return playerData.equipment.Cast<object>().ToList();
                default:
                    throw new System.Exception($"{tieredItemType} was not covered by get lootable list");
            }
        }

        
        private PlayerData deseralize(string data) {
            Debug.Log(Application.persistentDataPath);
            try {
                SPlayerData sPlayerData = JsonConvert.DeserializeObject<SPlayerData>(data);
                List<Equipment> equipment = DeseralizeEquipment(sPlayerData.equipmentIds);
                EquipCreatureSerializationFactory equipCreatureSerializationFactory = new EquipCreatureSerializationFactory();
                List<EquipedCreeture> equipedCreetures = equipCreatureSerializationFactory.deserializeList(sPlayerData.creatureData);
                List<IntItemSlot> lootboxCounts = new IntItemSlotSerializationFactory().deserializeList(sPlayerData.lootboxData);
                List<DoubleItemSlot> currencyCounts = new DoubleItemSlotSerializationFactory().deserializeList(sPlayerData.currencyCountData);
                List<IntItemSlot> craftingItems = new IntItemSlotSerializationFactory().deserializeList(sPlayerData.craftingItemData);
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
            string lootboxData = new IntItemSlotSerializationFactory().serialize(playerData.lootboxes);
            string creatureData = new EquipCreatureSerializationFactory().serialize(playerData.creetures);
            string currencyData = new DoubleItemSlotSerializationFactory().serialize(playerData.currencyCounts);
            string craftingItemData = new IntItemSlotSerializationFactory().serialize(playerData.craftingItems);
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
            IntItemSlot startingBox = new IntItemSlot(
                cardboardBox,
                9999
            );
            return new PlayerData(
                new List<string>(),
                null,
                new List<EquipedCreeture>(),
                new List<Equipment>(),
                new List<IntItemSlot>{startingBox},
                new List<DoubleItemSlot>(),
                new List<IntItemSlot>()
            );
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

        [System.Serializable]
        private class SPlayerData {
            public SPlayerData(
                string currentTileIndex, 
                List<string> discoveredTiles, 
                List<string> equipmentIds, 
                string creatureData,
                string lootboxData,
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
            public string lootboxData;
            public string currencyCountData;
            public string craftingItemData;
        }

        private class PlayerData {
            public PlayerData(
                List<string> discoveredTiles, 
                string currentTile, 
                List<EquipedCreeture> creetures, 
                List<Equipment> equipment, 
                List<IntItemSlot> lootboxCounts,
                List<DoubleItemSlot> currencyCounts,
                List<IntItemSlot> craftingItems
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
            public List<IntItemSlot> lootboxes;
            public List<DoubleItemSlot> currencyCounts;
            public List<IntItemSlot> craftingItems;
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
