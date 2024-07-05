using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions.Script {
    public static class ScriptCommandUtils 
    {
        public static IEnumerator executeCommand(CommandExecutionState state, ScriptCommand scriptCommand) {
            if (scriptCommand is InstantScriptCommand instantScriptCommand) {
                instantScriptCommand.execute(state);
            } else if (scriptCommand is DelayScriptCommand delayScriptCommand) {
                yield return delayScriptCommand.execute(state);
            }
        }
    }
}

