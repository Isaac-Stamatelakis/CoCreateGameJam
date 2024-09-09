using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions.Script.Execution {
    public class SelectionCommandLoop {
        public int Iterations {get; set;}
        public Stack<ScriptCommand> commands;
        public SelectionCommandLoop(Stack<ScriptCommand> commands, int iterations)
        {
            this.commands = commands;
            this.Iterations = iterations;
        }
        public void deIterate() {
            Iterations--;
        }
    }
}
