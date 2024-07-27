using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
    public class CraftingItem : Lootable
    {
        [SerializeField] private Sprite sprite;
        public override Sprite getSprite()
        {
            return sprite;
        }
    }
}

