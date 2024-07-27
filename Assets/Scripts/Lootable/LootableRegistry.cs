using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;

public class LootableRegistry
{
    private static LootableRegistry instance = null;
    private Dictionary<string, Lootable> dict;
    private LootableRegistry() {
        Lootable[] lootables = Resources.LoadAll<Lootable>("");
        dict = new Dictionary<string, Lootable>();
        foreach (Lootable lootable in lootables) {
            if (dict.ContainsKey(lootable.getId())) {
                Debug.LogError("Duplicate ID for " + lootable.name + " and " + dict[lootable.getId()].name);
                continue;
            }
            dict[lootable.getId()] = lootable;
        }
        Debug.Log(dict.Count + "Lootables Loaded");
    }
    public static LootableRegistry getInstance() {
        if (instance == null) {
            instance = new LootableRegistry();
        }
        return instance;
    }
    public Lootable getLootable(string id) {
        if (id == null || !dict.ContainsKey(id)) {
            return null;
        }
        return dict[id];
    }
    public T getLootable<T>(string id) where T : Lootable {
        Lootable lootable = getLootable(id);
        if (lootable == null || lootable is not T value) {
            return null;
        }
        return value;
    }
}
