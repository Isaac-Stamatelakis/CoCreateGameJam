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
        private CurrencyCount currencyCount;
        private TradingHistory tradingHistory;
        private float mood;
        public TradingCreature(EquipedCreeture equipedCreeture, CurrencyCount currencyCount, TradingHistory tradingHistory, float mood)
        {
            this.equipedCreeture = equipedCreeture;
            this.currencyCount = currencyCount;
            this.tradingHistory = tradingHistory;
            this.mood = mood;
        }

        public EquipedCreeture EquipedCreeture { get => equipedCreeture; set => equipedCreeture = value;}
        public Currency Currency { get => currencyCount==null ? null:currencyCount.currency;}
        public double Amount {get => currencyCount == null ? 0 : currencyCount.amount;}
        public float Mood {get => mood;}
        public CurrencyCount CurrencyCount { get => currencyCount; set => currencyCount = value; }
        public TradingHistory TradingHistory { get => tradingHistory; }

        public Sprite getSprite()
        {
            return equipedCreeture.getSprite();
        }

        public void tradeUpdate() {
            if (currencyCount == null || currencyCount.currency == null) {
                return;
            }
            double randomFactor = new System.Random().NextDouble() * 2 - 1;
            Currency currency = currencyCount.currency;
            double newAmount = currencyCount.amount + (currencyCount.amount * (currency.GrowthRate + randomFactor * currency.Volatility));
            currencyCount.amount = newAmount;
            tradingHistory.addTradingPeriod(newAmount);
        }
        public double getHourChange() {
            return 0;
        }

        public string getName()
        {
            return equipedCreeture.creeture.name;
        }
    }
    public static class TradingHistoryUtils {
        public static readonly int MAX_SECONDS = 120;
        public static readonly int MAX_MINUTES = 120;
        private static readonly List<TimePeriod> UpdatePeriods = new List<TimePeriod>{
            new TimePeriod(240,60),     // Seconds
            new TimePeriod(240,60),     // Minutes
            new TimePeriod(240,60),     // Hours
            new TimePeriod(int.MaxValue,60)// Days
        };
        private class TimePeriod {
            public int MAX_VALUE {get => max;}
            private int max;
            private int seperation; 
            public int SEPERATION {get => seperation;}
            public TimePeriod(int max, int seperation)
            {
                this.max = max;
                this.seperation = seperation;
            }
        }
        public static TradingHistory createNewHistory() {
            List<Queue<double>> queues = new List<Queue<double>>();
            for (int i = 0; i < UpdatePeriods.Count; i++) {
                queues.Add(new Queue<double>());
            }
            return new TradingHistory(queues,DateTime.UtcNow);
        }
        public static void updateTradingHistory(List<Queue<double>> queues, long runtime, double value) {
            for (int i = 0; i < UpdatePeriods.Count; i++) {
                Queue<double> queue = queues[i];
                TimePeriod timePeriod = UpdatePeriods[i];
                runtime = runtime % timePeriod.SEPERATION;
                if (runtime == 0) {
                    if (queue.Count > timePeriod.MAX_VALUE) {
                        queue.Dequeue();
                    }
                    queue.Enqueue(value);
                }
            }
        }
    }

    
    public class TradingHistory {
        public int TradingMinutes {get => DateTime.UtcNow.Minute - startTime.Minute;} 
        private DateTime startTime;
        private List<Queue<double>> queues;
        public TradingHistory(List<Queue<double>> queues,DateTime startTime)
        {
            this.queues = queues;
            this.startTime = startTime;
        }

        public void addTradingPeriod(double value) {
            TradingHistoryUtils.updateTradingHistory(queues,TradingController.Instance.RunTime,value);
        }
        public List<Queue<double>> deepCopyQueues() {
            List<Queue<double>> copyList = new List<Queue<double>>();
            foreach (Queue<double> queue in queues) {
                List<double> list = queue.ToList();
                Queue<double> copy = new Queue<double>();
                for (int i = list.Count-1;i >= 0; i--) {
                    copy.Enqueue(list[i]);
                }
                copyList.Add(copy);
            }
            return copyList;
        }
        public DateTime StartTime { get => startTime; }
    }

    public class STradingCreatureFactory : SerializationFactory<TradingCreature, SeralizedTradingCreature>
    {
        protected override TradingCreature deserializeValue(SeralizedTradingCreature sValue)
        {
            if (sValue == null) {
                return null;
            }
            Currency currency = CurrencyRegistry.getInstance().getCurrency(sValue.currencyId);
            TradingHistory tradingHistory = new TradingHistory(sValue.tradingHistory,sValue.startTime);
            
            CurrencyCount currencyCount = new CurrencyCount(currency,sValue.value);
            return new TradingCreature(
                new EquipCreatureSerializationFactory().deserialize(sValue.seralizedEquipedCreature),
                currencyCount,
                tradingHistory,
                sValue.mood
            );
        }
        protected override SeralizedTradingCreature formatValue(TradingCreature value)
        {
            string currencyId = value.Currency != null ? value.Currency.getId() : null;
            double currencyAmount = value.CurrencyCount != null ? value.CurrencyCount.amount : 0;
            return new SeralizedTradingCreature(
                new EquipCreatureSerializationFactory().serialize(value.EquipedCreeture),
                currencyId,
                value.TradingHistory.deepCopyQueues(),
                value.Mood,
                currencyAmount,
                value.TradingHistory.StartTime
            );
        }
        public TradingCreature initalize() {
            return new TradingCreature(
                null,
                null,
                TradingHistoryUtils.createNewHistory(),
                0
            );
        }
    }

    public class SeralizedTradingCreature {
        public string seralizedEquipedCreature;
        public string currencyId;
        public float mood;
        public double value;
        public List<Queue<double>> tradingHistory;
        public DateTime startTime;
        public SeralizedTradingCreature(
            string seralizedEquipedCreature, 
            string currencyId, 
            List<Queue<double>> tradingHistory, 
            float mood, 
            double value,
            DateTime startTime
        )
        {
            this.seralizedEquipedCreature = seralizedEquipedCreature;
            this.currencyId = currencyId;
            this.tradingHistory = tradingHistory;
            this.mood = mood;
            this.value = value;
            this.startTime = startTime;
        }
    }
}

