using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootBoxModule;

namespace CurrencyModule {
    [CreateAssetMenu(fileName = "New Currency", menuName = "Currency")]
    public class Currency : ScriptableObject, ILootable
    {
        public Sprite sprite;
        public int value;
    }

}
