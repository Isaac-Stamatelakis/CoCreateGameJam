using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class LootableUtils
{

    private static readonly List<string> amountPrefixes = new List<string>{
        "K",
        "M",
        "B",
        "T",
        "Qa",
        "Qi",
        "Sx",
        "Sp",
        "Oc",
        "No",
        "Dc",
        "UD",
        "DD",
        "TD",
        "QaD",
        "QnD",
        "SxD",
        "SpD",
        "OcD",
        "NnD",
        "Vi",
        "UVg",
        "DVg",
        "TVg",
        "QaVg",
        "QiVg",
        "SxVg",
        "SpVg",
        "OcVg",
        "NoVg",
        "Tg",
    };
    public static string formatAmount(double amount) {
        int powerOfTen = (int) Math.Floor(Math.Log10(amount));
        int prefixIndex = powerOfTen/3;
        if (amount < 1000000) {
            return $"{amount:F2}";
        }
        return amount.ToString("E2");
        /*
        Debug.Log(powerOfTen);
        if (prefixIndex > amountPrefixes.Count) {
            prefixIndex = amountPrefixes.Count-1;
        }   
        double normalizedValue = amount / Math.Pow(10,powerOfTen);
        int firstThreeDigits = (int)(normalizedValue * 100);
        return $"{firstThreeDigits}{amountPrefixes[prefixIndex]}";
        */
    }

    public static double parseAmount(string input) {
        double result;
        bool success = double.TryParse(input,out result);
        if (success) {
            return result;
        }
        return 0;
    }

    public static List<double> getQuickSelectOptions(double amount) {
        if (amount < 0.0001) {
            return new List<double>();
        }
        if (amount < 1) {
            return new List<double>{amount};
        }
        return new List<double>{
            amount/100d,
            amount/50d,
            amount/10d,
            amount/5d,
            amount
        };
    }
}
