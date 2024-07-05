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

        public override void addCommand(FormattedScriptCommand scriptCommand)
        {
            (ManaSubCommand subCommand, float amount, ManaParamType paramType, float range, bool self) = ManaCommand.parse(scriptCommand);
            if (subCommand == ManaSubCommand.remove) {
                amount *= -1;
            }
            bool steal = subCommand == ManaSubCommand.steal;
            switch (paramType) {
                case ManaParamType.num:
                    constantManaValues.modify(amount,range,true);
                    break;
                case ManaParamType.max:
                    maxManaValues.modify(amount,true);
                    break;
                case ManaParamType.current:
                    currentManaValues.modify(amount,true);
                    break;
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
            string description = "";
            if (!maxManaValues.isInert()) {
                description += $"{maxManaValues.Percent*100}% of their max mana";
            }
            if (!currentManaValues.isInert()) {
                string currentManaValueDescription = $"{currentManaValues.Percent*100}% of their current mana";
                if (description.Length == 0) {
                    description += currentManaValueDescription;
                } else {
                    description += $" plus {currentManaValueDescription}";
                }
            }
            if (!constantManaValues.isInert()) {
                string constantManaValueDescription = $"{constantManaValues.Min}-{constantManaValues.Max}";
                if (description.Length == 0) {
                    description += constantManaValueDescription;
                } else {
                    description += $" plus {constantManaValueDescription}";
                }
            }
            return new List<string>{
                description
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
            public float Steal;
            public void modify(float amount, float range, bool steal) {
                Min += amount - range;
                Max += amount + range;
                if (steal) {
                    Steal += amount;
                }
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
            public void modify(float percent, bool steal) {
                Percent += percent;
                if (steal) {
                    Steal += percent;
                }
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

