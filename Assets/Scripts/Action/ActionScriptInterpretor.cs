using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Levels.Combat;
using Creatures.Actions;
using System;

namespace Actions.Script {
    public static class ActionScriptInterpretor
    {
        public static string getDescription(string script) {
            return null;
        }

        public static T[] reduceArraySize<T>(T[] arr, int count) {
            T[] parameters = new T[arr.Length-count];
            for (int i = count; i < arr.Length; i++) {
                parameters[i-count] = arr[i];
            }
            return parameters;
        }

        public static Stack<ScriptCommand> parseCommands(string script) {
            Stack<ScriptCommand> commandStack = new Stack<ScriptCommand>();
            string[] lines = script.Split(";");
            for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++) {
                string line = lines[lineIndex];
                string[] splitLine = line.Split(" ");
                if (splitLine.Length == 0) {
                    continue;
                }
                string command = splitLine[0];
                string[] parameters = reduceArraySize<string>(splitLine,1);
                commandStack.Push(new ScriptCommand(
                    command,
                    parameters,
                    lineIndex
                ));
            
            }
            
            return commandStack;
        }

        
    }

    public class ScriptCommand {
        private int line;
        private string command;
        private string[] parameters;
        public ScriptCommand(string command, string[] parameters, int line) {
            this.command = command;
            this.parameters = parameters;
            this.line = line;
        }

        public string Command { get => command;}
        public string[] Parameters { get => parameters;}
        public int Line {get => line;}
    }
    public class CommandExecutionState {
            private Stack<ScriptCommand> commandStack;
            private HashSet<string> scriptGroupExecutions = new HashSet<string>();
            private Dictionary<string, GameObject> objectPrefabs;
            private Dictionary<string, AnimationClip> animations;
            private GameObject spawnedObject;
            private CreatureSelector creatureSelector;
            public bool pausedForSelection;

            public CommandExecutionState(ScriptedAction scriptedAction)
            {
                this.commandStack = ActionScriptInterpretor.parseCommands(scriptedAction.ActionScript);
                this.objectPrefabs = scriptedAction.PrefabDict;
                this.animations = scriptedAction.AnimationDict;
            }

            public void executeSection() {
                pausedForSelection = false;
                while (commandStack.Count > 0) {
                    ScriptCommand scriptCommand = commandStack.Pop();
                    executeCommand(scriptCommand);
                }
            }
            public CreatureSelector getCurrentSelector() {
                return creatureSelector;
            }
            private void executeCommand(ScriptCommand scriptCommand) {
                switch (scriptCommand.Command) {
                    case "start":
                        executeStart(scriptCommand);
                        break;
                    case "end":
                        executeEnd(scriptCommand);
                        break;
                }
            }

            private void executeStart(ScriptCommand scriptCommand) {
                if (scriptCommand.Parameters.Length == 0) {
                    scriptError(scriptCommand,"no sub command provided to start");
                }
                string subCommand = scriptCommand.Parameters[0];
                string[] subParameters = ActionScriptInterpretor.reduceArraySize<string>(scriptCommand.Parameters,1);
                scriptGroupExecutions.Remove(subCommand);
                switch (subCommand) {
                    case "select":
                        Dictionary<string,object> parsedParameters = parseParameters(
                            parseInstructions: new List<ParseInstruction>{
                                new ParseInstruction(ParseType.Integer,"targets",true),
                                new ParseInstruction(ParseType.String,"type",true)
                            },
                            parameters: subParameters,
                            scriptCommand: scriptCommand
                        );
                        TargetType targetType = GlobalUtils.stringToEnum<TargetType>((string)parsedParameters["type"]);
                        creatureSelector = new CreatureSelector(
                            targetType: targetType,
                            maxTargets: (int)parsedParameters["targets"]
                        );
                        pausedForSelection = true;
                        break;
                    case "spawn":
                        break;
                    default:
                        scriptError(scriptCommand,$"{subCommand} is not a valid start command");
                        break;
                }
            }
            private Dictionary<string,object> parseParameters(List<ParseInstruction> parseInstructions, string[] parameters, ScriptCommand scriptCommand) {
                Dictionary<string,object> dict = new Dictionary<string, object>();
                Dictionary<string, ParseInstruction> parameterToInstruction = new Dictionary<string, ParseInstruction>();
                HashSet<string> requiredParameters = new HashSet<string>();
                HashSet<string> usedParameters = new HashSet<string>();
                foreach (ParseInstruction parseInstruction in parseInstructions) {
                    parameterToInstruction[parseInstruction.Key] = parseInstruction;
                    if (parseInstruction.Required) {
                        requiredParameters.Add(parseInstruction.Key);
                    }
                }
                foreach (string parameter in parameters) {
                    string[] splitParameter = parameter.Split("=");
                    if (splitParameter.Length == 0) {
                        scriptError(scriptCommand, $"parameter {parameter} is formatted incorrectly. Format as parameter=val");
                    }
                    if (splitParameter.Length == 1) {
                        scriptError(scriptCommand, $"parameter {parameter} has no provided value. Format as parameter=val");
                    }
                    if (splitParameter.Length > 2) {
                        scriptError(scriptCommand, $"parameter {parameter} has more than one '='");
                    }
                    string parameterName = splitParameter[0];
                    string parameterVal = splitParameter[1];
                    if (!parameterToInstruction.ContainsKey(parameterName)) {
                        scriptError(scriptCommand, $"parameter {parameterName} is not valid in this context");
                    }
                    if (requiredParameters.Contains(parameterName)) {
                        requiredParameters.Remove(parameterName);
                    }
                    if (usedParameters.Contains(parameterName)) {
                        scriptError(scriptCommand,$"parameter {parameterName} is used multiple times");
                    }
                    usedParameters.Add(parameterName);
                    switch (parameterToInstruction[parameterName].ParseType) {
                        case ParseType.Integer:
                            try {
                                dict[parameterName] = Convert.ToInt32(parameterVal);
                            } catch (FormatException) {
                                scriptError(scriptCommand,$"parameter {parameter} recieved a non integer value {parameterVal}");
                            }
                            break;
                        case ParseType.String:
                            dict[parameterName] = parameterVal;
                            break;
                    }
                }
                return dict;
            }

            private void scriptError(ScriptCommand scriptCommand, string description) {
                throw new System.Exception($"Script execution error at line {scriptCommand.Line} {description}");
            }

            private void executeEnd(ScriptCommand scriptCommand) {
                if (scriptCommand.Parameters.Length == 0) {
                    scriptError(scriptCommand,"no sub command provided to end");
                }
                string subCommand = scriptCommand.Parameters[0];
                if (!scriptGroupExecutions.Contains(subCommand)) {
                    scriptError(scriptCommand,$"end {subCommand} was called prior to start");
                }
                scriptGroupExecutions.Remove(subCommand);
                switch (subCommand) {
                    case "select":
                        break;
                    case "spawn":
                        break;
                    default:
                        scriptError(scriptCommand,$"{subCommand} is not a valid end command");
                        break;
                }
            }

            

            private enum ParseType {
                Integer,
                String
            }

            private class ParseInstruction {
                public ParseType ParseType;
                public string Key;
                public bool Required;
                public ParseInstruction(ParseType parseType, string key, bool required)
                {
                    ParseType = parseType;
                    Key = key;
                    Required = required;
                }
            }
        }
}

