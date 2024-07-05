using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions.Script.Execution;

namespace Actions.Script {
    public interface IScriptCommand {
        public FormattedScriptCommand getFormattedScriptCommand();
    }
    public abstract class ScriptCommand : IScriptCommand
    {
        protected FormattedScriptCommand formattedScriptCommand;
        public ScriptCommand(FormattedScriptCommand formattedScriptCommand) {
            this.formattedScriptCommand = formattedScriptCommand;
        }

        public FormattedScriptCommand getFormattedScriptCommand()
        {
            return formattedScriptCommand;
        }
    }
}

