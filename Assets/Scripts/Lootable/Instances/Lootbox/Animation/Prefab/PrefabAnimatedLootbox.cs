using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LootBoxes {
    [CreateAssetMenu(fileName = "New Lootbox", menuName = "Lootbox/Prefab")]
    public class PrefabAnimatedLootbox : LootBox
    {
        [SerializeField] private LootBoxAnimation animation;

        public LootBoxAnimation Animation { get => animation; }
    }
}

