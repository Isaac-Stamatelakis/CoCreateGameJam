using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootBoxes;
using Items;

namespace Trading {
    [CreateAssetMenu(fileName = "New Currency", menuName = "Item/Currency")]
    public class Currency : Lootable
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private string acroynm;
        [SerializeField] private double value;
        [SerializeField] private double hourlyGrowthRate;
        [SerializeField] private double volatility;
        public double Value { get => value; }
        public double GrowthRate { get => hourlyGrowthRate / Global.TRADE_UPDATES_PER_HOUR;}
        public double Volatility { get => volatility; }
        public string Acroynm { get => acroynm; }

        public override ItemSlotType getItemSlotType()
        {
            return ItemSlotType.Double;
        }

        public override Sprite getSprite()
        {
            return sprite;
        }
    }

}
