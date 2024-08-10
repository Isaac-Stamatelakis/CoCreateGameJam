using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
    [System.Serializable]
    public abstract class ItemSlot : IDisplayable
    {
        [SerializeField] private Lootable lootable;
        public ItemSlot(Lootable lootable)
        {
            this.lootable = lootable;
        }
        public Lootable Lootable { get => lootable; }
        public string getName()
        {
            return lootable.getName();
        }

        public Sprite getSprite()
        {
            return lootable.getSprite();
        }

        public bool match(ItemSlot itemSlot) {
            if (itemSlot == null || itemSlot.lootable == null || lootable == null) {
                return false;
            }
            return itemSlot.lootable.getId().Equals(lootable.getId());
        }
    }

}
