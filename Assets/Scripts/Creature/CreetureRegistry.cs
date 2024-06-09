using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures {
    public class CreatureRegistry
        {
            private static CreatureRegistry instance = null;
            private Dictionary<string, Creature> dict;

            private CreatureRegistry() {
                Creature[] creetures = Resources.LoadAll<Creature>("Creetures/");
                dict = new Dictionary<string, Creature>();
                foreach (Creature creeture in creetures) {
                    if (dict.ContainsKey(creeture.Id)) {
                        Debug.LogError("Duplicate ID for " + creeture.name + " and " + dict[creeture.Id].name);
                        continue;
                    }
                    dict[creeture.Id] = creeture;
                }
                Debug.Log(dict.Count + " Creetures Loaded");
            }
            public static CreatureRegistry getInstance() {
                if (instance == null) {
                    instance = new CreatureRegistry();
                }
                return instance;
            }

            public Creature getCreature(string id) {
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
