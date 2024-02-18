using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CreatureModule {
    public class CreetureRegistry
        {
            private static CreetureRegistry instance = null;
            private Dictionary<string, Creeture> dict;

            private CreetureRegistry() {
                Creeture[] creetures = Resources.LoadAll<Creeture>("Creetures/");
                dict = new Dictionary<string, Creeture>();
                foreach (Creeture creeture in creetures) {
                    if (dict.ContainsKey(creeture.id)) {
                        Debug.LogError("Duplicate ID for " + creeture.name + " and " + dict[creeture.id].name);
                        continue;
                    }
                    dict[creeture.id] = creeture;
                }
                Debug.Log(dict.Count + " Creetures Loaded");
            }
            public static CreetureRegistry getInstance() {
                if (instance == null) {
                    instance = new CreetureRegistry();
                }
                return instance;
            }

            public Creeture getEquipment(string id) {
                if (id == null) {
                    return null;
                }
                if (dict.ContainsKey(id)) {
                    return dict[id];
                }
                return null;
            }

        }
}
