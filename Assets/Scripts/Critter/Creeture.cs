using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootBoxModule;
using InventoryModule;

namespace CreatureModule {
    [CreateAssetMenu(fileName = "New Level Creeture", menuName = "Creature")]
    [System.Serializable]
    public class Creature : ScriptableObject, ILootable, IItem
    {
        public Sprite sprite;
        public string id;
    }
}

