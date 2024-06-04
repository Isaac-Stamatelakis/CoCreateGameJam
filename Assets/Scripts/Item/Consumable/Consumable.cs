using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.Consumables {
    public abstract class Consumable : ScriptableObject, IDisplayable
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private string id;
        public Sprite getSprite()
        {
            return sprite;
        }
    }
}

