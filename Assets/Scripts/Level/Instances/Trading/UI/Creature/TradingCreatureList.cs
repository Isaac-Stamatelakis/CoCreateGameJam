using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Inventory;

namespace Trading.UI {
    public class TradingCreatureList : InventoryUI<TradingCreature>
    {
        [SerializeField] private CreatureTradingUIPage creatureTradingUIPage;
        public CreatureTradingUIPage CreatureTradingUIPage { get => creatureTradingUIPage;}

        public override void leftClick(int index)
        {
            
        }

        public override void rightClick(int index)
        {
            
        }
    }
}

