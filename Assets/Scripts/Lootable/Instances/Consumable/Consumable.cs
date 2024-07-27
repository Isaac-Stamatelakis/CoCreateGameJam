using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.Consumables {
    public abstract class Consumable : Lootable
    {
        [SerializeField] private Sprite sprite;

        public override Sprite getSprite()
        {
            return sprite;
        }

        
    }
}

