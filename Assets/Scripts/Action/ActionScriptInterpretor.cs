using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Levels.Combat;
using Creatures.Actions;
using System;
using System.Linq;

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
            private Dictionary<string,GameObject> spawnedObjects = new Dictionary<string, GameObject>();
            private CreatureSelector creatureSelector;
            private bool pausedForSelection;
            private CreatureCombatObject selfCreature;
            private SelectionCommandLoop subStack;
            public bool Complete {get => commandStack.Count==0 && subStack.iterations==0;}
            public CommandExecutionState(ScriptedAction scriptedAction, CreatureCombatObject selfCreature)
            {
                this.commandStack = ActionScriptInterpretor.parseCommands(scriptedAction.ActionScript);
                this.objectPrefabs = scriptedAction.PrefabDict;
                this.animations = scriptedAction.AnimationDict;
                this.selfCreature = selfCreature;
            }

            public IEnumerator executeSection() {
                if (subStack != null) {
                    subStack.iterations = creatureSelector.Creatures.Count;
                    while (subStack.iterations > 0) {
                        Queue<ScriptCommand> tempQueue = new Queue<ScriptCommand>();
                        while (subStack.commands.Count > 0) {
                            ScriptCommand command = subStack.commands.Pop();
                            tempQueue.Enqueue(command);
                            yield return executeCommand(command);
                        }
                        
                        while (tempQueue.Count > 0) {
                            subStack.commands.Push(tempQueue.Dequeue());
                        }
                        subStack.iterations--;
                    }
                }
                pausedForSelection = false;
                while (commandStack.Count > 0 && !pausedForSelection) {
                    ScriptCommand scriptCommand = commandStack.Pop();
                    yield return executeCommand(scriptCommand);
                }
            }
            public CreatureSelector getCurrentSelector() {
                return creatureSelector;
            }
            private IEnumerator executeCommand(ScriptCommand scriptCommand) {
                Debug.Log(scriptCommand.Command);
                switch (scriptCommand.Command) {
                    case "start":
                        executeStart(scriptCommand);
                        break;
                    case "end":
                        executeEnd(scriptCommand);
                        break;
                    case "move":
                        yield return executeMove(scriptCommand);
                        break;
                    case "attack":
                        executeAttack(scriptCommand);
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
                        Stack<ScriptCommand> reversedStack = new Stack<ScriptCommand>();
                        bool foundEnd = false;
                        while (commandStack.Count > 0) {
                            ScriptCommand command = commandStack.Pop();
                            reversedStack.Push(command);
                            if (!command.Command.Equals("end")) {
                                continue;
                            }
                            if (command.Parameters.Length == 0) {
                                scriptError(command,"end command has no command to end");
                            }
                            if (!command.Parameters[0].Equals("select")) {
                                continue;
                            }
                            foundEnd = true;
                            break;
                        }
                        if (!foundEnd) {
                            scriptError(scriptCommand,"Select command had no end statement");
                        }
                        Stack<ScriptCommand> subCommandStack = new Stack<ScriptCommand>();
                        reversedStack.Pop(); // Remove end
                        while (reversedStack.Count > 0) {
                            subCommandStack.Push(reversedStack.Pop());
                        }
                        subStack = new SelectionCommandLoop(subCommandStack);
                        break;
                    case "spawn":
                        break;
                    default:
                        scriptError(scriptCommand,$"{subCommand} is not a valid start command");
                        break;
                }
            }

            private void executeAttack(ScriptCommand scriptCommand) {
                Dictionary<string,object> parameters = parseParameters(
                    new List<ParseInstruction>{
                        new ParseInstruction(ParseType.Float,
                        "min",
                        true
                        ),
                        new ParseInstruction(ParseType.Float,
                        "max",
                        true
                        ),
                        new ParseInstruction(ParseType.String,
                        "type",
                        false
                        )
                    },
                    scriptCommand.Parameters,
                    scriptCommand
                );
                DamageType damageType = DamageType.Physical;
                if (parameters.ContainsKey("type")) {
                    string damageString = (string) parameters["type"];
                    damageType = GlobalUtils.stringToEnum<DamageType>(damageString);
                }
                float min = (float)parameters["min"];
                float max = (float)parameters["max"];
                float damage = UnityEngine.Random.Range(min,max);
                CreatureCombatObject target = getTarget();
                target.CreatureInCombat.hit(damage,damageType);
            }

            private IEnumerator executeMove(ScriptCommand scriptCommand) {
                if (scriptCommand.Parameters.Length == 0) {
                    scriptError(scriptCommand,"No object to move given");
                }
                if (scriptCommand.Parameters.Length == 1) {
                    scriptError(scriptCommand,"No position to move given");
                }
                string toMoveString = scriptCommand.Parameters[0];
                Transform toMoveTransform = null;
                if (toMoveString.Equals("self")) {
                    toMoveTransform = selfCreature.transform;
                } else {
                    if (!spawnedObjects.ContainsKey(toMoveString)) {
                        scriptError(scriptCommand,$"Could not find object {toMoveString}");
                    }
                    toMoveTransform = spawnedObjects[toMoveString].transform;
                }
                string toGoString = scriptCommand.Parameters[1];
                Vector3 toGoPosition = Vector3.zero;
                if (toGoString.Equals("self")) {
                    toGoPosition = selfCreature.transform.position;
                } else if (toGoString.Equals("target")) {
                    toGoPosition = getTarget().transform.position;
                } else {
                    try {
                        List<float> exactPosition = parseArray(toGoString);
                        if (exactPosition.Count != 2) {
                            scriptError(scriptCommand,"Exact position to go to must have 2 values");
                        }
                        toGoPosition = new Vector3(exactPosition[0],exactPosition[1],0);
                    } catch (FormatException) {
                        scriptError(scriptCommand,"To go position was not 'self','target' or '[x,y]'");
                    }
                }
                string[] additionalParameters = ActionScriptInterpretor.reduceArraySize<string>(scriptCommand.Parameters,2);
                Dictionary<string,object> parsedParameters = parseParameters(
                    new List<ParseInstruction>{
                        new ParseInstruction(ParseType.Integer,
                        "vel",
                        false
                        ),
                        new ParseInstruction(ParseType.IntegerArray,
                        "off",
                        false
                        )
                    },
                    additionalParameters,
                    scriptCommand
                );
                float vel = 1;
                if (parsedParameters.ContainsKey("vel")) {
                    vel = (int) parsedParameters["vel"];
                }
                if (parsedParameters.ContainsKey("off")) {
                    List<float> offsetList = (List<float>) parsedParameters["off"];
                    if (offsetList.Count != 2) {
                        scriptError(scriptCommand,"Offset array must have exactly 2 values");
                    }
                    toGoPosition = toGoPosition + new Vector3(offsetList[0],offsetList[1],0);
                }
                Vector3 toMovePosition = toMoveTransform.position;
                (Vector3,int) tuple = GlobalUtils.speedAndIterationsToMove(toGoPosition,toMovePosition,vel);
                Vector3 speed = tuple.Item1;
                int iterations = tuple.Item2;
                if (toMoveString.Equals("self")) {
                    while (iterations > 0) {
                        iterations--;
                        selfCreature.move(speed);
                        yield return new WaitForFixedUpdate();
                    }
                } else {
                    while (iterations > 0) {
                        iterations--;
                        toMoveTransform.position += speed;
                        yield return new WaitForFixedUpdate();
                    }
                }
                
            }

            private CreatureCombatObject getTarget() {
                return creatureSelector.Creatures[subStack.iterations-1];
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
                        case ParseType.Float:
                            try {
                                dict[parameterName] = (float)Convert.ToDouble(parameterVal);
                            } catch (FormatException) {
                                scriptError(scriptCommand,$"parameter {parameter} recieved a non float value {parameterVal}");
                            }
                            break;
                        case ParseType.String:
                            dict[parameterName] = parameterVal;
                            break;
                        case ParseType.IntegerArray:
                            try {
                                dict[parameterName] = parseArray(parameterVal);
                            } catch (FormatException) {
                                scriptError(scriptCommand,$"parameter {parameter} recieved a non array value {parameterVal}");
                            }
                            break;
                    }
                }
                return dict;
            }

            private List<float> parseArray(string val) {
                string[] values = val.Split(",");
                values[0] = values[0].Remove(0);
                values[values.Length-1] = values[values.Length-1].Remove(values[values.Length-1].Length-1);
                List<float> array = new List<float>();
                for (int i = 0; i < values.Length; i++) {
                    values[i] = values[i].Replace(" ","");
                    array.Add((float)Convert.ToDouble(values[i]));
                }
                return array;
            }

            private void scriptError(ScriptCommand scriptCommand, string description) {
                throw new System.Exception($"Script execution error at line {scriptCommand.Line} '{description}'");
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
                Float,
                String,
                IntegerArray
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

            private class SelectionCommandLoop {
                public int iterations;
                public Stack<ScriptCommand> commands;

                public SelectionCommandLoop(Stack<ScriptCommand> commands)
                {
                    iterations = -1;
                    this.commands = commands;
                }
            }
        }
}

