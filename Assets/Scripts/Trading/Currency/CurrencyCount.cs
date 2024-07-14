using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootBoxes;
namespace Trading {
    public class CurrencyCount : Lootable
    {
        public Currency currency;
        public double amount;
        public CurrencyCount(Currency currency, double amount)
        {
            this.currency = currency;
            this.amount = amount;
        }

        public override Sprite getSprite()
        {
            return currency.getSprite();
        }
    }

    public class SCurrencyCount {
        public double amount;
        public string currencyId;

        public SCurrencyCount(string currencyId, double amount)
        {
            this.amount = amount;
            this.currencyId = currencyId;
        }
    }

    public class CurrencyCountSerializer : SerializationFactory<CurrencyCount, SCurrencyCount>
    {
        protected override CurrencyCount deserializeValue(SCurrencyCount sValue)
        {
            return new CurrencyCount(
                CurrencyRegistry.getInstance().getCurrency(sValue.currencyId),
                sValue.amount
            );
        }

        protected override SCurrencyCount formatValue(CurrencyCount value)
        {
            string currencyID = value == null ? null : value.currency.getId();
            return new SCurrencyCount(
                currencyID,
                value.amount
            );
        }
    }
}


