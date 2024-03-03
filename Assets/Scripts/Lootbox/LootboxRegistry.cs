using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LootBoxModule {

    public class LootBoxRegistry 
    {
        private static LootBoxRegistry instance;
        private Dictionary<string, LootBox> dict;
        private LootBoxRegistry() {
            LootBox[] creetures = Resources.LoadAll<LootBox>("Lootbox/");
            dict = new Dictionary<string, LootBox>();
            foreach (LootBox lootBox in creetures) {
                if (dict.ContainsKey(lootBox.id)) {
                    Debug.LogError("Duplicate ID for " + lootBox.name + " and " + dict[lootBox.id].name);
                    continue;
                }
                dict[lootBox.id] = lootBox;
            }
            Debug.Log(dict.Count + " Lootboxes Loaded");
        }
        public static LootBoxRegistry getInstance() {
            if (instance == null) {
                instance = new LootBoxRegistry();
            }
            return instance;
        }

        public LootBox getLootbox(string id) {
            if (dict.ContainsKey(id)) {
                return dict[id];
            }
            return null;
        }
    }
}
