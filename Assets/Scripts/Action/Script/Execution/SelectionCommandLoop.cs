using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions.Script.Execution {
    public class SelectionCommandLoop {
        public int iterations;
        public Stack<ScriptCommand> commands;
        public SelectionCommandLoop(Stack<ScriptCommand> commands)
        {
            iterations = -1;
            this.commands = commands;
        }
    }
}
