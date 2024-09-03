using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Actions.Script {
    public static class ScriptCommandFactory 
    {
        public static ScriptCommand convertFormattedCommand(FormattedScriptCommand formattedScriptCommand) {
            switch (formattedScriptCommand.Command) {
                case "mana":
                    return new ManaCommand(formattedScriptCommand);
                case "end":
                    return new EndCommand(formattedScriptCommand);
                case "select":
                    return new SelectCommand(formattedScriptCommand);
                case "spawn":
                    return new SpawnCommand(formattedScriptCommand);
                case "if":
                    return new IfCommand(formattedScriptCommand);
                case "move":
                    return new MoveCommand(formattedScriptCommand);
                case "attack":
                    return new AttackCommand(formattedScriptCommand);
                case "action":
                    return new ActionCommand(formattedScriptCommand);
                case "animation":
                    return new AnimationCommand(formattedScriptCommand);
                case "sound":
                    return new SoundCommand(formattedScriptCommand);
                case "status":
                    return new StatusCommand(formattedScriptCommand);
                case "heal":
                    return new HealCommand(formattedScriptCommand);
                case "chance":
                    return new ChanceCommand(formattedScriptCommand);
                case "selectf":
                    return new SpecialSelectCommand(formattedScriptCommand);
                default:
                    ActionScriptInterpretorUtils.scriptError(formattedScriptCommand,$"{formattedScriptCommand.Command} is not a valid command");
                    break;
            }
            return null;
        }
        public static Stack<ScriptCommand> parseCommands(string script) {
            Stack<ScriptCommand> commandStack = new Stack<ScriptCommand>();
            if (script == null) {
                return new Stack<ScriptCommand>();
            }
            string[] lines = script.Split(";");
            for (int lineIndex = lines.Length-1; lineIndex >= 0; lineIndex--) {
                string line = lines[lineIndex];
                line = line.Replace("\n","");
                string[] splitLine = line.Split(" ");
                if (splitLine.Length == 0) {
                    continue;
                }
                string command = splitLine[0];
                if (command.Length == 0) {
                    continue;
                }
                List<string> splitLineNoEmpty = new List<string>();
                foreach (string parameter in splitLine) {
                    if (parameter.Length > 0) {
                        splitLineNoEmpty.Add(parameter);
                    }
                }
                string[] parameters = new string[splitLineNoEmpty.Count-1];
                for (int i = 0; i < splitLineNoEmpty.Count-1; i++) {
                    parameters[i] = splitLineNoEmpty[i+1];
                }
                FormattedScriptCommand formattedScriptCommand = new (
                    command,
                    parameters,
                    lineIndex
                );
                commandStack.Push(convertFormattedCommand(formattedScriptCommand));
            }
            return commandStack;
        }
    }
}

