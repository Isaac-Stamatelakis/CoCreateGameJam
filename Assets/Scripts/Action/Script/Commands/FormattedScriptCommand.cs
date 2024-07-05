using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions.Script {
    public class FormattedScriptCommand {
        private int line;
        protected string command;
        protected string[] parameters;
        public FormattedScriptCommand(string command, string[] parameters, int line) {
            this.command = command;
            this.parameters = parameters;
            this.line = line;
        }

        public string Command { get => command;}
        public string[] Parameters { get => parameters;}
        public int Line {get => line;}
    }
}