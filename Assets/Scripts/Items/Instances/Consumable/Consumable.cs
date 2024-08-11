using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.Consumables {
    [CreateAssetMenu(fileName = "New Consumable", menuName = "Item/Consumable")]
    public class Consumable : Lootable
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private string actionScript;
        public override Sprite getSprite()
        {
            return sprite;
        }

        public override ItemSlotType getItemSlotType()
        {
            return ItemSlotType.Int;
        }

        
    }
}

