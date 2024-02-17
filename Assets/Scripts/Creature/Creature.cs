using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootBoxModule;

namespace CreatureModule {
    [CreateAssetMenu(fileName = "New Level Creature", menuName = "Creature")]
    [System.Serializable]
    public class Creature : ScriptableObject, ILootable
    {
        public Sprite sprite;
        public string id;
    }
}

