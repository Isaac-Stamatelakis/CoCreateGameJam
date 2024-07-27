using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
    [System.Serializable]
    public class ItemSlot : IDisplayable
    {
        [SerializeField] private Lootable lootable;
        [SerializeField] private int amount;

        public ItemSlot(Lootable lootable, int amount)
        {
            this.lootable = lootable;
            this.amount = amount;
        }

        public int Amount { get => amount; }
        public Lootable Lootable { get => lootable; }

        public string getName()
        {
            return lootable.name;
        }

        public Sprite getSprite()
        {
            return lootable.getSprite();
        }
    }

}
