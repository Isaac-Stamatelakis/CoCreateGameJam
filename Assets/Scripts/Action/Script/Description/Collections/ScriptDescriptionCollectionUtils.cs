using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions.Script {
    public static class ScriptDescriptionCollectionUtils
    {
        public static List<string> getActionOrder() {
            return new List<string>{
                "attack",
                "heal",
                "status",
                "mana"
            };
        }

        public static ActionDescriptionCollection getActionCollection(string command,ActionTargetType actionTargetType) {
            switch (command) {
                case "attack":
                    return new AttackDescriptionCollection(actionTargetType);
                case "heal":
                    return new HealDescriptionCollection();
                case "status":
                    return new StatusDescriptionCollection();
                case "mana":
                    return new ManaDescriptionCollection();
                default:
                    return null;
            }
        }

        public static string integerToText(int n) {
            string[] numbers = {
                "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten",
                "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen", "twenty",
                "twenty-one", "twenty-two", "twenty-three", "twenty-four", "twenty-five", "twenty-six", "twenty-seven", "twenty-eight", "twenty-nine", "thirty",
                "thirty-one", "thirty-two", "thirty-three", "thirty-four", "thirty-five", "thirty-six", "thirty-seven", "thirty-eight", "thirty-nine", "forty",
                "forty-one", "forty-two", "forty-three", "forty-four", "forty-five", "forty-six", "forty-seven", "forty-eight", "forty-nine", "fifty"
            };
            if (n < 0 || n > numbers.Length) {
                return n.ToString();
            }
            return numbers[n];
            
        }
    }
}

