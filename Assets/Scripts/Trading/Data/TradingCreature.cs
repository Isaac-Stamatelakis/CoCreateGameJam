using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Trading;
using System;
using System.Linq;
using Newtonsoft.Json;

namespace Trading {
    public class TradingCreature : IDisplayable
    {
        private EquipedCreeture equipedCreeture;
        private Currency currency;
        private DateTime startTime;
        private List<double> tradingHistory;
        private float mood;
        public TradingCreature(EquipedCreeture equipedCreeture, Currency currency, List<double> tradingHistory, float mood)
        {
            this.equipedCreeture = equipedCreeture;
            this.currency = currency;
            this.tradingHistory = tradingHistory;
            this.mood = mood;
        }

        public EquipedCreeture EquipedCreeture { get => equipedCreeture; set => equipedCreeture = value;}
        public Currency Currency { get => currency; }
        public List<double> TradingHistory { get => tradingHistory; }
        public double Amount {get => tradingHistory.Last();}
        public float Mood {get => mood;}
        public int TradingMinutes {get => (int) (startTime - DateTime.UtcNow).TotalMinutes;}

        public Sprite getSprite()
        {
            return equipedCreeture.getSprite();
        }

        public void tradeUpdate() {
            if (currency == null) {
                return;
            }
            double randomFactor = new System.Random().NextDouble() * 2 - 1;
            double newAmount = Amount + (Amount * (currency.GrowthRate + randomFactor * currency.Volatility));
            tradingHistory.Add(newAmount);
        }
        public double getHourChange() {
            return 0;
        }

        public string getName()
        {
            return equipedCreeture.creeture.name;
        }
    }

    public class STradingCreatureFactory : SerializationFactory<TradingCreature, SeralizedTradingCreature>
    {
        protected override TradingCreature deserializeValue(SeralizedTradingCreature sValue)
        {
            if (sValue == null) {
                return null;
            }
            return new TradingCreature(
                new EquipCreatureSerializationFactory().deserialize(sValue.seralizedEquipedCreature),
                CurrencyRegistry.getInstance().getCurrency(sValue.currencyId),
                sValue.tradingHistory,
                sValue.mood
            );
        }
        protected override SeralizedTradingCreature formatValue(TradingCreature value)
        {
            string currencyId = value.Currency != null ? value.Currency.getId() : null;
            return new SeralizedTradingCreature(
                new EquipCreatureSerializationFactory().serialize(value.EquipedCreeture),
                currencyId,
                value.TradingHistory,
                value.Mood
            );
        }
        public TradingCreature initalize() {
            return new TradingCreature(
                null,
                null,
                new List<double>{0},
                0
            );
        }
    }

    public class SeralizedTradingCreature {
        public string seralizedEquipedCreature;
        public string currencyId;
        public float mood;
        public List<double> tradingHistory;
        public SeralizedTradingCreature(string seralizedEquipedCreature, string currencyId, List<double> tradingHistory, float mood)
        {
            this.seralizedEquipedCreature = seralizedEquipedCreature;
            this.currencyId = currencyId;
            this.tradingHistory = tradingHistory;
            this.mood = mood;
        }
    }
}

