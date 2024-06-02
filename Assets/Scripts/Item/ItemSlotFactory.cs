using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Creatures;

namespace Items {
    public static class ItemSlotFactory 
    {
        public static Color getColor(Rarity rarity) {
            switch (rarity) {
                case Rarity.Common:
                    return new Color(1f,1f,1f,100f/255f);
                case Rarity.Exotic:
                    return new Color(1f,1f,1f,150f/255f);
                case Rarity.Rare:
                    return new Color(1f,1f,1f,200f/255f);
                case Rarity.Legendary:
                    return new Color(1f,1f,1f,1f);
            }
            return Color.white;
        }

        public static Sprite getSprite(Rarity rarity) {
            switch (rarity) {
                case Rarity.Common:
                    return Resources.Load<Sprite>("Sprites/Borders/Common_border");
                case Rarity.Exotic:
                    return Resources.Load<Sprite>("Sprites/Borders/Exotic_border");
                case Rarity.Rare:
                    return Resources.Load<Sprite>("Sprites/Borders/Rare_border");
                case Rarity.Legendary:
                    return Resources.Load<Sprite>("Sprites/Borders/Legendary_border");
            }
            return null;
        }
    }
}

