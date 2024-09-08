using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions.Script {
    public abstract class DelayScriptCommand : ScriptCommand
    {
        public DelayScriptCommand(FormattedScriptCommand formattedScriptCommand) : base(formattedScriptCommand)
        {
        }
        public abstract IEnumerator execute(LiveCommandExecutionState commandExecutionState);
    }
}

