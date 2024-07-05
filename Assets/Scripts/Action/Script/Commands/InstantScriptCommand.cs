using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions.Script {
    public abstract class InstantScriptCommand : ScriptCommand
    {
        public InstantScriptCommand(FormattedScriptCommand formattedScriptCommand) : base(formattedScriptCommand)
        {
        }

        public abstract void execute(CommandExecutionState commandExecutionState);
    }
}

