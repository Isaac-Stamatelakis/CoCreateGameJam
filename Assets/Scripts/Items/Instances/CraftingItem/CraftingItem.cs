using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
    [CreateAssetMenu(fileName = "New Crafting Item", menuName = "Item/Crafting")]
    public class CraftingItem : Lootable
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private string description;

        public string Description { get => description; }

        public override Sprite getSprite()
        {
            return sprite;
        }
    }
}

