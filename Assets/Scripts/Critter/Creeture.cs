using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootBoxModule;
using InventoryModule;

namespace CreatureModule {
    [CreateAssetMenu(fileName = "New Level Creeture", menuName = "Creeture")]
    [System.Serializable]
    public class Creeture : ScriptableObject, ILootable, IItem
    {
        public Sprite sprite;
        public string id;
        private List<Equipment> equipment;
        public List<Equipment> Equipment { get => equipment; set => equipment = value; }
    }
}

