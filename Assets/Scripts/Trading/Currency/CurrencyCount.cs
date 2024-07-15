using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootBoxes;
using System;
using System.Linq;

namespace Trading {
    public class CurrencyCount : ILootableCount, IDisplayable
    {
        public Currency currency;
        public double amount;
        public CurrencyCount(Currency currency, double amount)
        {
            this.currency = currency;
            this.amount = amount;
        }

        public void addAmount(float amount)
        {
            this.amount += amount;
        }

        public float getAmount()
        {
            return (float) amount;
        }

        public string getName()
        {
            return currency.name;
        }

        public Sprite getSprite()
        {
            return currency.getSprite();
        }

        Lootable ILootableCount.getLootable()
        {
            return currency;
        }
    }

    public static class CurrencyCountUtils {
        private static double SUFFIX_MULTIPLIER = 1000;
        private static readonly List<char> suffixes = new List<char>
        {
            'K',     // Thousand
            'M',     // Million
            'B',     // Billion
            'T',     // Trillion
            'Q',     // Quadrillion
            'q',     // Quintillion
            'S',     // Sextillion
            's',     // Septillion
            'O',     // Octillion
            'N',     // Nonillion
            'D'      // Decillion
        };

        public static string format(double input) {
            if (input < SUFFIX_MULTIPLIER) {
                return $"{input:F2}";
            }
            double threshold = 1;
            foreach (var suffix in suffixes)
            {
                threshold *= SUFFIX_MULTIPLIER;
                if (input < threshold)
                {
                    return formatSuffix(input,threshold,suffix);
                }
            }
            var largestSuffix = suffixes.Last();
            return formatSuffix(input,threshold,largestSuffix);
        }

        private static string formatSuffix(double input, double powerOfTen, char suffix) {
            double value = input/powerOfTen;
            return $"{value:F2}{suffix}";
        } 
        public static double parse(string input) {
            try {
                return Convert.ToDouble(input);
            } catch (FormatException) {
                return parseWithSuffix(input);
            }
        }

        public static List<double> getQuickSelectOptions(double input) {
            char suffix = getSuffix(input);
            bool noSuffix = suffixes.Contains(suffix);
            List<double> values = new List<double>();
            if (noSuffix) {
                List<double> possibleValues = new List<double>{
                    10,
                    50,
                    100,
                    500
                };
                foreach (double possibleValue in possibleValues) {
                    if (input < possibleValue) {
                        values.Add(possibleValue);
                    }
                }
                values.Add(input);
                return values;
            }
            int index = suffixes.IndexOf(suffix);
            int seperator = index / 4;
            for (int i = 0; i < index; i++) {
                values.Add(getPowerOfTen(suffixes[i*seperator]));
            }
            values.Add(getPowerOfTen(suffix));
            values.Add(input);
            return values;
        }

        private static char getSuffix(double input) {
            if (input < SUFFIX_MULTIPLIER) {
                return default(char);
            }
            double threshold = 1;
            foreach (char suffix in suffixes) {
                threshold *= SUFFIX_MULTIPLIER;
                if (input < threshold) {
                    return suffix;
                }
            }
            return suffixes.Last();
        }

        private static double getPowerOfTen(char suffix) {
            if (!suffixes.Contains(suffix)) {
                return double.NaN;
            }
            int index = suffixes.IndexOf(suffix);
            return Mathf.Pow(10,3*(index+1));
        }

        private static double parseWithSuffix(string input) {
            char last = input.Last();
            if (!suffixes.Contains(last)) {
                return double.NaN;
            }
            try {
                string subString = input.Substring(0,input.Length-1);
                double parsed = Convert.ToDouble(subString);
                return getPowerOfTen(last) * parsed;
            } catch (FormatException) {
                return double.NaN;
            }
            
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


