using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Trading {
    public class CurrencyRegistry
    {
        private static CurrencyRegistry instance = null;
        private Dictionary<string, Currency> dict;
        private CurrencyRegistry() {
            Currency[] currencies = Resources.LoadAll<Currency>("Currencies/");
            dict = new Dictionary<string, Currency>();
            foreach (Currency currency in currencies) {
                if (dict.ContainsKey(currency.getId())) {
                    Debug.LogError("Duplicate ID for " + currency.name + " and " + dict[currency.getId()].name);
                    continue;
                }
                dict[currency.getId()] = currency;
            }
            Debug.Log(dict.Count + " Currencies Loaded");
        }
        public static CurrencyRegistry getInstance() {
            if (instance == null) {
                instance = new CurrencyRegistry();
            }
            return instance;
        }
        public Currency getCurrency(string id) {
            if (id == null) {
                return null;
            }
            if (dict.ContainsKey(id)) {
                return dict[id];
            }
            return null;
        }
        public Currency[] getAll() {
            return dict.Values.ToArray();
        }
    }
}

public interface IIdedObject {
    public string getId();
}
