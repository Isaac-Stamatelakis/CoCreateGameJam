using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Actions.Script {
    public class AttackDescriptionCollection : ActionDescriptionCollection
    {
        public AttackDescriptionCollection(ActionTargetType actionTargetType) {
            this.actionTargetType = actionTargetType;
        }
        private ActionTargetType actionTargetType;
        private List<string> descriptions = new List<string>();
        public override void addCommand(FormattedScriptCommand scriptCommand)
        {
            (float damage, float range, DamageType damageType, float falloff, float lifesteal, bool self) = AttackCommand.parse(scriptCommand);
            float min = damage-range;
            float max = damage+range;
            string damageTypeString = damageType.ToString().ToLower();
            string description = $"{min:F1}-{max:F1} {damageTypeString} damage";
            descriptions.Add(description);
        }
        public override List<string> getDescription() {
            return descriptions;
        }
        public override string getPrefix(bool passive)
        {
            return passive! ? "dealing" : "deals";
        }
        public override string getSuffix(bool passive)
        {
            if (actionTargetType == ActionTargetType.Self) {
                return " to themself";
            }
            return null;
        }
    }
}

