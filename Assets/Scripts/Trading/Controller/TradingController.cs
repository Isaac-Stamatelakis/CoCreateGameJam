using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Trading;
using WorldCreationModule;
using System.IO;
using Newtonsoft.Json;
using System;
using Creatures;
using System.Linq;

namespace Trading {
    public class TradingController : MonoBehaviour
    {
        private static TradingController instance;
        public static TradingController Instance { get => instance;}
        public List<TradingCreature> TradingCreatures { get => tradingCreatures; }
        private List<TradingCreature> tradingCreatures;
        private Dictionary<string, CurrencyTradingData> currencyData;
        private DateTime lastLogoff;
        private long previousPlayTime;
        public long RunTime {get => previousPlayTime + (lastLogoff.Second - DateTime.UtcNow.Second);}
        private int updateCounter;
        public void Awake() {
            instance = this;
            readData();
        }

        private void readData() {
            string dataPath = WorldCreation.getTradingDataPath(Global.WorldName);
            Debug.Log(Application.persistentDataPath);
            if (!File.Exists(dataPath)) {
                initTradingData();
                return;
            }
            string json = File.ReadAllText(dataPath);
            SeralizedTradingData seralizedTradingData = JsonConvert.DeserializeObject<SeralizedTradingData>(json);
            this.updateCounter = seralizedTradingData.updateCounter;
            /*
            currencyData = new Dictionary<string, CurrencyTradingData>();
            foreach (CurrencyTradingData currencyTradingData in seralizedTradingData.currencyTradingData) {
                currencyData[currencyTradingData.currencyID] = currencyTradingData;
            }
            */
            tradingCreatures = new STradingCreatureFactory().deserializeList(seralizedTradingData.seralizedTradingCreatures);
        }

        private void initTradingData() {
            // Default 2 trading creatures
            STradingCreatureFactory tradingCreatureFactory = new STradingCreatureFactory();
            tradingCreatures = new List<TradingCreature>{
                tradingCreatureFactory.initalize(),
                tradingCreatureFactory.initalize()
            };
            /*
            currencyData = new Dictionary<string, CurrencyTradingData>();
            Currency[] currencies = CurrencyRegistry.getInstance().getAll();
            foreach (Currency currency in currencies) {
                decimal initalValue = (decimal) currency.StartingValue;
                currencyData[currency.getId()] = new CurrencyTradingData(
                    currency.getId(),
                    new List<decimal>{initalValue}
                );
            }
            */
        }
        public void FixedUpdate() {
            updateCounter++;
            if (updateCounter > Global.TRADE_UPDATE_FREQ) {
                updateCounter = 0;
                foreach (TradingCreature tradingCreature in tradingCreatures) {
                    if (tradingCreature == null) {
                        continue;
                    }
                    tradingCreature.tradeUpdate();
                }
            }
        }
        private void tradeUpdate() {
            
        }

        public void OnDestroy() {
            string path = WorldCreation.getTradingDataPath(Global.WorldName);
            SeralizedTradingData seralizedTradingData = new SeralizedTradingData(
                seralizedTradingCreatures: new STradingCreatureFactory().serialize(tradingCreatures),
                serializedExchangeRates: null,
                closedTime: DateTime.UtcNow,
                updateCounter: updateCounter
            );
            File.WriteAllText(path, JsonConvert.SerializeObject(seralizedTradingData));
        }

        public CurrencyTradingData getTradingData(string id) {
            if (!currencyData.ContainsKey(id)) {
                return null;
            }
            return currencyData[id];
        }

        private class SeralizedTradingData {
            public string seralizedTradingCreatures;
            public string serializedExchangeRates;
            public DateTime closedTime;
            public int updateCounter;
            public SeralizedTradingData(string seralizedTradingCreatures, string serializedExchangeRates, DateTime closedTime, int updateCounter)
            {
                this.seralizedTradingCreatures = seralizedTradingCreatures;
                this.serializedExchangeRates = serializedExchangeRates;
                this.closedTime = closedTime;
                this.updateCounter = updateCounter;
            }
        }
    }



}
