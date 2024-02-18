using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace WorldCreationModule {
public static class WorldCreation
    {
        public static bool worldExists(string name) {
            string path = getWorldPath(name);
            return folderExists(path);
        }

        public static bool folderExists(string path) {
            return File.Exists(path) || Directory.Exists(path);
        }

        public static void createWorld(string name) {
            string path = getWorldPath(name);
            Directory.CreateDirectory(path);
            Debug.Log("World Folder Created at " + path);
            initPlayerData(name);
        }
        public static string getWorldPath(string name) {
            return Application.persistentDataPath + "/worlds/" + name; 
        }

        public static void initPlayerData(string name) {
            /*
            PlayerData playerData = new PlayerData(
                x: 0,
                y: 0,
                robotID: "happy_mk1",
                name: "Izakio",
                inventoryJson: ItemSlotFactory.createEmptySerializedInventory(40)
            );
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(playerData);
            string path = getPlayerDataPath(name);
            File.WriteAllText(path,json);
            */
        }

        public static string getPlayerDataPath(string name) {
            return getWorldPath(name) + "/player_data.json";;
        }
    }
}

