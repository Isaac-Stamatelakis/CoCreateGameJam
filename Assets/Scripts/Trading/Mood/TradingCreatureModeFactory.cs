using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trading {
    public enum TradingCreatureMood {
        Neutral,
        Happy,
        Joyful,
        Jubilant,
        Euphoric,
        Annoyed,
        Sad,
        Depressed,
        Desolate
    }
    public static class TradingCreatureModeUtils
    {
        public static string getMoodDescription(TradingCreatureMood mood) {
            return mood switch {
                TradingCreatureMood.Neutral => "Doing the Grind",
                _ => ""
            };
        }

        public static TradingCreatureMood getMood(float mood) {
            if (mood < 1 || mood > -1) {
                return TradingCreatureMood.Neutral;
            }
            if (mood >= 0) {
                if (mood < 4) {
                    return TradingCreatureMood.Happy;
                }
                if (mood < 8) {
                    return TradingCreatureMood.Joyful;
                }
                if (mood < 16) {
                    return TradingCreatureMood.Jubilant;
                }
                return TradingCreatureMood.Euphoric;
            } else {
                if (mood > -4) {
                    return TradingCreatureMood.Annoyed;
                }
                if (mood > -8) {
                    return TradingCreatureMood.Sad;
                }
                if (mood > -16) {
                    return TradingCreatureMood.Depressed;
                }
                return TradingCreatureMood.Desolate;
            }
        }
    }
}

