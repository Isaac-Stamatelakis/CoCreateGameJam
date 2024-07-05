using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions.Script {
    public abstract class ActionDescriptionCollection
    {
        public abstract void addCommand(FormattedScriptCommand scriptCommand);
        public abstract List<string> getDescription();
        public abstract string getPrefix(bool passive);
        public abstract string getSuffix(bool passive);

    }

}
