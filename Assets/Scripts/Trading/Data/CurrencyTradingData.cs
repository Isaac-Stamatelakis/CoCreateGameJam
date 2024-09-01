using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Newtonsoft.Json;

namespace Trading {
    public class CurrencyTradingData
    {
        public string currencyID;
        public List<decimal> priceHistory;

        public CurrencyTradingData(string currencyID, List<decimal> priceHistory)
        {
            this.currencyID = currencyID;
            this.priceHistory = priceHistory;
        }

        [JsonIgnore] public decimal CurrentPrice {get => priceHistory.Last();}
    }
}

