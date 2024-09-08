using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions;
using Actions.Script.Execution;

namespace Actions.Script {
    public static class CommandExecutionStateUtils
    {
        public static void generateSubStack(LiveCommandExecutionState commandExecutionState) {
            Stack<ScriptCommand> reversedStack = new Stack<ScriptCommand>();
            while (commandExecutionState.CommandStack.Count > 0) {
                ScriptCommand command = commandExecutionState.CommandStack.Pop();
                reversedStack.Push(command);
                if (command is not EndCommand endCommand) {
                    continue;
                }
                if (!endCommand.endsSelect()) {
                    continue;
                }
                reversedStack.Pop(); // Remove end
                break;
            }
            Stack<ScriptCommand> subCommandStack = new Stack<ScriptCommand>();
            while (reversedStack.Count > 0) {
                subCommandStack.Push(reversedStack.Pop());
            }
            commandExecutionState.SubStack = new SelectionCommandLoop(subCommandStack);
        }
    }

}
