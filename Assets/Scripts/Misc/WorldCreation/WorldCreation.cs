using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Player;

namespace WorldCreationModule {
public static class WorldCreation
    {
        private static readonly string playerDataPath = "player_data.json";
        private static readonly string tradingDataPath = "trading_data.json";
        public static bool worldExists(string name) {
            string path = getWorldPath(name);
            return folderExists(path);
        }

        public static string getWorldName(int index) {
            return "world" + index.ToString();
        }
        public static bool folderExists(string path) {
            return File.Exists(path) || Directory.Exists(path);
        }

        public static void createWorld(string name) {
            string path = getWorldPath(name);
            Directory.CreateDirectory(path);
            Debug.Log("World Folder Created at " + path);
        }
        public static string getWorldPath(string name) {
            return Application.persistentDataPath + "/worlds/" + name; 
        }

        public static void initPlayerData(string name,string playerData) {
            string path = getPlayerDataPath(name);
            File.WriteAllText(path,playerData);  
        }

        public static string getPlayerDataPath(string name) {
            return Path.Combine(getWorldPath(name),playerDataPath);
        }
        public static string getTradingDataPath(string name) {
            return Path.Combine(getWorldPath(name),tradingDataPath);
        }
    }
}

