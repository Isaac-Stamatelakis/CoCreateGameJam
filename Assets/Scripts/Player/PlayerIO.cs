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
using Items.Equipment;

namespace Player {
    public class PlayerIO : MonoBehaviour
    {
        private static PlayerIO instance;
        private List<EquipedCreeture> combatCreatures;
        public List<EquipedCreeture> EquipedCreetures {get => playerData.creetures; set => playerData.creetures = value;}
        public List<EnchantedEquipment> Equipment {get => playerData.equipment; set => playerData.equipment = value;}
        public List<IntItemSlot> LootBoxes {get => ItemSlotUtils.sortList<LootBox,IntItemSlot>(playerData.intItemSlots);}
        public List<IntItemSlot> CraftingItems {get => ItemSlotUtils.sortList<CraftingItem,IntItemSlot>(playerData.intItemSlots);}
        private PlayerData playerData;
        public string CurrentTile {get => playerData.currentTile; set => playerData.currentTile = value;}
        public static PlayerIO Instance { get => instance;}
        public List<DoubleItemSlot> Currencies {get=> ItemSlotUtils.sortList<Currency,DoubleItemSlot>(playerData.doubleItemSlots);}

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

        public bool hasItems(List<IntItemSlot> itemSlots) {
            foreach (IntItemSlot itemSlot in itemSlots) {
                if (!hasItem(itemSlot)) {
                    return false;
                }
            }
            return true;
        }

        public void take(List<IntItemSlot> itemSlots) {
            foreach (IntItemSlot itemSlot in itemSlots) {
                take(itemSlot);
            }
        }
        public void give(IntItemSlot itemSlot) {
            if (itemSlot == null || itemSlot.Lootable == null) {
                return;
            }
            Lootable lootable = itemSlot.Lootable;
            ItemSlotType itemSlotType = lootable.getItemSlotType();
            switch (itemSlotType) {
                case ItemSlotType.Int:
                    ItemSlotUtils.insertList<IntItemSlot>(playerData.intItemSlots,itemSlot);
                    break;
                case ItemSlotType.Double:
                    DoubleItemSlot currencySlot = ItemSlotUtils.fromIntItemSlot(itemSlot);
                    ItemSlotUtils.insertList<DoubleItemSlot>(playerData.doubleItemSlots,currencySlot);
                    Debug.Log(currencySlot.Amount);
                    break;
                case ItemSlotType.Unique:
                    PlayerIOUtils.giveLootable(playerData,lootable);
                    break;
            }
        }

        public bool hasItem(IntItemSlot intItemSlot) {
            if (intItemSlot == null || intItemSlot.Lootable == null) {
                return false;
            }
            Lootable lootable = intItemSlot.Lootable;
            ItemSlotType itemSlotType = lootable.getItemSlotType();
            switch (itemSlotType) {
                case ItemSlotType.Int:
                    return ItemSlotUtils.hasAmount<IntItemSlot>(playerData.intItemSlots,lootable.getId(),intItemSlot.Amount);
                case ItemSlotType.Double:
                    return ItemSlotUtils.hasAmount<DoubleItemSlot>(playerData.doubleItemSlots,lootable.getId(),intItemSlot.Amount);
            }
            return false;
        }
        public void take(IntItemSlot intItemSlot) {
            if (intItemSlot == null || intItemSlot.Lootable == null) {
                return;
            }
            Lootable lootable = intItemSlot.Lootable;
            ItemSlotType itemSlotType = lootable.getItemSlotType();
            switch (itemSlotType) {
                case ItemSlotType.Int:
                    ItemSlotUtils.takeAmount<IntItemSlot>(playerData.intItemSlots,lootable.getId(),intItemSlot.Amount);
                    break;
                case ItemSlotType.Double:
                    ItemSlotUtils.takeAmount<DoubleItemSlot>(playerData.doubleItemSlots,lootable.getId(),intItemSlot.Amount);
                    break;
            }
        }

        public int amountOfItem(string id) {
            Lootable lootable = LootableRegistry.getInstance().getLootable(id);
            ItemSlotType itemSlotType = lootable.getItemSlotType();
            switch (itemSlotType) {
                case ItemSlotType.Int:
                    return ItemSlotUtils.getAmount<IntItemSlot>(playerData.intItemSlots,id);
                case ItemSlotType.Double:
                    return ItemSlotUtils.getAmount<DoubleItemSlot>(playerData.doubleItemSlots,id);
                case ItemSlotType.Unique:
                    if (lootable is Creature) {
                        return getAmountOfUniqueItem(id,TieredItemType.Creature,null);
                    } else if (lootable is Equipment) {
                        return getAmountOfUniqueItem(id,TieredItemType.Equipment,null);
                    }
                    break;
            }
            return 0;
        }



        public int getAmountOfUniqueItem(string id, TieredItemType tieredItemType, Rarity? rarity) {
            switch (tieredItemType) {
                case TieredItemType.Creature:
                    return ItemSlotUtils.getAmountFromList(playerData.creetures.Cast<UniqueItemSlot>().ToList(),id,rarity);
                case TieredItemType.Equipment:
                    return ItemSlotUtils.getAmountFromList(playerData.equipment.Cast<UniqueItemSlot>().ToList(),id,rarity);
            }
            return 0;
            
        }
        
        public void take(TieredItemType tieredItemType, int index) {
            List<object> elements = getLootableList(tieredItemType);
            if (index < 0 || index >= elements.Count) {
                return;
            }
            elements.RemoveAt(index);
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
                List<EnchantedEquipment> equipment = new EquipmentFactory().deserializeList(sPlayerData.equipmentData);
                EquipCreatureSerializationFactory equipCreatureSerializationFactory = new EquipCreatureSerializationFactory();
                List<EquipedCreeture> equipedCreetures = equipCreatureSerializationFactory.deserializeList(sPlayerData.creatureData);
                List<IntItemSlot> intItemSlots = new IntItemSlotSerializationFactory().deserializeList(sPlayerData.intItemSlots);
                List<DoubleItemSlot> doubleItemSlots = new DoubleItemSlotSerializationFactory().deserializeList(sPlayerData.doubleItemSlots);
                return new PlayerData(
                    currentTile: sPlayerData.currentTile,
                    discoveredTiles: sPlayerData.discoveredTiles,
                    creetures: equipedCreetures,
                    equipment: equipment,
                    intItemSlots: intItemSlots,
                    doubleItemSlots: doubleItemSlots
                );
            } catch (JsonSerializationException e) {
                Debug.LogError($"Error during player deseralization {e}");
                return initPlayerData();
            }
            
            
        }

        public string seralize() {
            string equipmentData = new EquipmentFactory().serialize(playerData.equipment);
            string creatureData = new EquipCreatureSerializationFactory().serialize(playerData.creetures);
            string doubleItemSlotData = new DoubleItemSlotSerializationFactory().serialize(playerData.doubleItemSlots);
            string intItemSlotData = new IntItemSlotSerializationFactory().serialize(playerData.intItemSlots);
            SPlayerData sPlayerData = new SPlayerData(
                currentTileIndex: playerData.currentTile,
                discoveredTiles: playerData.discoveredTiles,
                equipmentData: equipmentData,
                creatureData: creatureData,
                intItemSlots: intItemSlotData,
                doubleItemSlots: doubleItemSlotData
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
                new List<EnchantedEquipment>(),
                new List<IntItemSlot>{startingBox},
                new List<DoubleItemSlot>()
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
                string equipmentData, 
                string creatureData,
                string intItemSlots,
                string doubleItemSlots
            ) {
                this.currentTile = currentTileIndex;
                this.discoveredTiles = discoveredTiles;
                this.equipmentData = equipmentData;
                this.creatureData = creatureData;
                this.intItemSlots = intItemSlots;
                this.doubleItemSlots = doubleItemSlots;
            }
            public string currentTile; 
            public List<string> discoveredTiles;
            public string equipmentData;
            public string creatureData;
            public string intItemSlots;
            public string doubleItemSlots;
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

    public class PlayerData {
        public PlayerData(
            List<string> discoveredTiles, 
            string currentTile, 
            List<EquipedCreeture> creetures, 
            List<EnchantedEquipment> equipment, 
            List<IntItemSlot> intItemSlots,
            List<DoubleItemSlot> doubleItemSlots
            ) {
            this.discoveredTiles = discoveredTiles;
            this.currentTile = currentTile;
            this.creetures = creetures;
            this.equipment = equipment;
            this.intItemSlots = intItemSlots;
            this.doubleItemSlots = doubleItemSlots;
        }
        public List<string> discoveredTiles;
        public string currentTile;
        public List<EquipedCreeture> creetures;
        public List<EnchantedEquipment> equipment;
        public List<IntItemSlot> intItemSlots;
        public List<DoubleItemSlot> doubleItemSlots;
    }
}
