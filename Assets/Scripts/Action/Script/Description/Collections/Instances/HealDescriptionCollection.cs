using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Actions.Script {
    public class HealDescriptionCollection : ActionDescriptionCollection
    {
        private float min;
        private float max;
        public override void addCommand(ScriptCommand scriptCommand)
        {
            (float heal, float range) = ActionScriptCommandParser.parseHealCommand(scriptCommand);
            min += heal-range;
            max += heal+range;
        }
        public override List<string> getDescription()
        {
            return new List<string>{
                $"{min:F1}-{max:F1} health"
            };
        }

        public override string getPrefix(bool passive)
        {
            return  passive! ? "healing" : "heals";
        }

        public override string getSuffix(bool passive)
        {
            return null;
        }
    }
}

