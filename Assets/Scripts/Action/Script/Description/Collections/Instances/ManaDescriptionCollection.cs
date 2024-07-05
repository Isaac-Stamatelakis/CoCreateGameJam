using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Actions.Script {
    public class ManaDescriptionCollection : ActionDescriptionCollection
    {
        private ConstantManaValues constantManaValues;
        private PercentManaValues maxManaValues;
        private PercentManaValues currentManaValues;
        public ManaDescriptionCollection()
        {
            constantManaValues = new ConstantManaValues();
            maxManaValues = new PercentManaValues();
            currentManaValues = new PercentManaValues();
        }

        public override void addCommand(ScriptCommand scriptCommand)
        {
            (float amount, float range, float stealPercent, bool percent, bool currentMana) = ActionScriptCommandParser.parseManaCommand(scriptCommand);
            if (percent) {
                if (currentMana) {
                    currentManaValues.modify(amount,stealPercent);
                } else {
                    maxManaValues.modify(amount,stealPercent);
                }
            } else {
                constantManaValues.modify(amount,range,stealPercent);
            }
        }
        private PrefixType getPrefixType(List<IManaValue> manaValues) {
            bool allPositive = true;
            foreach (IManaValue manaValue in manaValues) {
                if (manaValue.isInert()) {
                    continue;
                }
                if (!manaValue.isPositive()) {
                    allPositive = false;
                    break;
                }
            }
            if (allPositive) {
                return PrefixType.Increase;
            }
            bool allNegative = true;
            foreach (IManaValue manaValue in manaValues) {
                if (manaValue.isInert()) {
                    continue;
                }
                if (manaValue.isPositive()) {
                    allNegative = false;
                    break;
                }
            }
            if (allNegative) {
                return PrefixType.Decrease;
            }
            return PrefixType.Modify;
        }

        private string getPrefix(PrefixType prefixType, bool passive) {
            switch (prefixType) {
                case PrefixType.Decrease:
                    return passive! ? "reducing" : "reduces";
                case PrefixType.Increase:
                    return passive! ? "increasing" : "increases";
                case PrefixType.Modify:
                    return passive! ? "modifying" : "modifies";
                default:
                    throw new System.Exception($"{prefixType} not covered");
            }
        }
        private List<IManaValue> GetManaValues() {
            return new List<IManaValue>{
                constantManaValues,
                maxManaValues,
                currentManaValues
            };
        }
        public override List<string> getDescription()
        {
            List<IManaValue> manaValues = GetManaValues();
            string fullDescription = "";
            if (!maxManaValues.isInert()) {
                fullDescription += $"{maxManaValues.Percent*100}% of their max mana";
            }
            if (!currentManaValues.isInert()) {
                string currentManaValueDescription = $"{currentManaValues.Percent*100}% of their current mana";
                if (fullDescription.Length == 0) {
                    fullDescription += currentManaValueDescription;
                } else {
                    fullDescription += $" plus {currentManaValueDescription}";
                }
            }
            if (!constantManaValues.isInert()) {
                string constantManaValueDescription = $"{constantManaValues.Min}-{constantManaValues.Max}";
                if (fullDescription.Length == 0) {
                    fullDescription += constantManaValueDescription;
                } else {
                    fullDescription += $" plus {constantManaValueDescription}";
                }
            }
            return new List<string>{
                fullDescription
            };
        }
        public override string getPrefix(bool passive)
        {
            List<IManaValue> manaValues = GetManaValues();
            PrefixType prefixType = getPrefixType(manaValues);
            string prefix = getPrefix(prefixType,passive);
            return $"{prefix} mana by";
        }

        public override string getSuffix(bool passive)
        {
            return null;
        }

        private class ConstantManaValues : IManaValue {
            public float Min;
            public float Max;
            public float StealPercent;
            public void modify(float amount, float range, float steal) {
                Min += amount - range;
                Max += amount + range;
            }
            public bool isPositive() {
                return (Min+Max)/2 >= 0;
            }
            public bool isInert() {
                return Min+Max==0;
            }
        }
        private class PercentManaValues : IManaValue {
            public float Percent;
            public float Steal;
            public void modify(float percent, float steal) {
                Percent += percent;
                Steal += percent * steal;
            }
            public bool isPositive() {
                return Percent >= 0;
            }
            public bool isInert() {
                return Percent==0;
            }
        }

        private interface IManaValue {
            public bool isPositive();
            public bool isInert();
        }

        private enum PrefixType {
            Increase,
            Modify,
            Decrease
        }
    }
}

