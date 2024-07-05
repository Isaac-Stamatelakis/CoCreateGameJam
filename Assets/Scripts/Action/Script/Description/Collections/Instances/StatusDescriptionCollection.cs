using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions.Script {
    public class StatusDescriptionCollection : ActionDescriptionCollection
    {
        public override void addCommand(ScriptCommand scriptCommand)
        {
            throw new System.NotImplementedException();
        }

        public override List<string> getDescription()
        {
            return new List<string>();
        }

        public override string getPrefix(bool passive)
        {
            return null;
        }

        public override string getSuffix(bool passive)
        {
            return null;
        }
    }
}

