using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootBoxes;

namespace CurrencyModule {
    [CreateAssetMenu(fileName = "New Currency", menuName = "Currency")]
    public class Currency : Lootable
    {
        public Sprite sprites;
        public int value;

        public override Sprite getSprite()
        {
            return sprites;
        }
    }

}
