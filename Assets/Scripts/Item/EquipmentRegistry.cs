using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryModule {
    public class EquipmentRegistry
    {
        private static EquipmentRegistry instance = null;
        private Dictionary<string, Equipment> equipmentDict;

        private EquipmentRegistry() {
            Equipment[] equipments = Resources.LoadAll<Equipment>("Equipment/");
            equipmentDict = new Dictionary<string, Equipment>();
            foreach (Equipment equipment in equipments) {
                if (equipmentDict.ContainsKey(equipment.id)) {
                    Debug.LogError("Duplicate ID for " + equipment.name + " and " + equipmentDict[equipment.id].name);
                    continue;
                }
                equipmentDict[equipment.id] = equipment;
            }
            Debug.Log(equipmentDict.Count + " Equipment Items Loaded");
        }
        public static EquipmentRegistry getInstance() {
            if (instance == null) {
                instance = new EquipmentRegistry();
            }
            return instance;
        }

        public Equipment getEquipment(string id) {
            if (id == null) {
                return null;
            }
            if (equipmentDict.ContainsKey(id)) {
                return equipmentDict[id];
            }
            return null;
        }

    }
}

