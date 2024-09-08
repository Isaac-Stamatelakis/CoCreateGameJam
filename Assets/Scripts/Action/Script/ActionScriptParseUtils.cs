using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Creatures;
using Items;
using Levels.Combat;
using Actions.Script.Execution;

namespace Actions.Script {
    public static class ActionScriptParseUtils 
    {
        public static List<float> parseArray(string val) {
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

        public static double parseDoubleValue(string val, FormattedScriptCommand scriptCommand, ICommandExecutionState commandExecutionState) {
            try {
                return Convert.ToDouble(val);
            } catch (FormatException) {

            }
            try {
                string[] split = val.Split(".");
                string objIndicator = split[0];
                string attributeIndicator = split[1];
                ICombatCreature creatureInCombat = commandExecutionState.getCreatureFromIndicator(scriptCommand,objIndicator);
                return ActionScriptParseUtils.parseCreatureAttribute(scriptCommand,attributeIndicator,creatureInCombat);
            } catch (IndexOutOfRangeException) {
                ActionScriptInterpretorUtils.scriptError(scriptCommand,$"{val} must be of form obj.val");
            }
            return default(double);
        }
        

        public static double parseCreatureAttribute(FormattedScriptCommand scriptCommand, string attribute, ICombatCreature creatureInCombat) {
            switch (attribute) {
                case "health":
                    return creatureInCombat.getHealth();
                case "health_percent":
                    return creatureInCombat.getHealthPercent();
                case "mana":
                    return creatureInCombat.getMana();
                case "mana_percent":
                    return creatureInCombat.getManaPercent();
                case "speed":
                    return creatureInCombat.getStat(CreatureStat.Speed);
                case "attack":
                    return creatureInCombat.getStat(CreatureStat.Attack);
                case "defense":
                    return creatureInCombat.getStat(CreatureStat.Armor);
                case "ability_power":   
                    return creatureInCombat.getStat(CreatureStat.Ability);
                default:
                    ActionScriptInterpretorUtils.scriptError(scriptCommand,$"{attribute} is not a valid attribute");
                    return default(double);
            }
        }
        public static T[] reduceArraySize<T>(T[] arr, int count) {
            T[] parameters = new T[arr.Length-count];
            for (int i = count; i < arr.Length; i++) {
                parameters[i-count] = arr[i];
            }
            return parameters;
        }
        public static object parse(ParseType parseType, string val, string parameterDescription, FormattedScriptCommand scriptCommand) {
            switch (parseType) {
                case ParseType.Integer:
                    try {
                        return Convert.ToInt32(val);
                    } catch (FormatException) {
                        ActionScriptInterpretorUtils.scriptError(scriptCommand,$"parameter {parameterDescription} recieved a non integer value {val}");
                    }
                    break;
                case ParseType.Float:
                    try {
                        return (float)Convert.ToDouble(val);
                    } catch (FormatException) {
                        ActionScriptInterpretorUtils.scriptError(scriptCommand,$"parameter {parameterDescription} recieved a non float value {val}");
                    }
                    break;
                case ParseType.String:
                    return val;
                case ParseType.IntegerArray:
                    try {
                        return ActionScriptParseUtils.parseArray(val);
                    } catch (FormatException) {
                        ActionScriptInterpretorUtils.scriptError(scriptCommand,$"parameter {parameterDescription} recieved a non array value {val}");
                    }
                    return null;
                case ParseType.Boolean:
                    if (val.Equals("true")) {
                        return true;
                    } else if (val.Equals("false")) {
                        return false;
                    } else {
                        ActionScriptInterpretorUtils.scriptError(scriptCommand,$"parameter {parameterDescription} recieved a non boolean value {val}");
                        return null;
                    }
            }
            return null;
        }

        public static List<object> parseOrdered(List<ParseInstruction> parseInstructions, string[] parameters, FormattedScriptCommand scriptCommand) {
            List<object> objects = new List<object>();
            for (int i = 0; i < parseInstructions.Count; i++) {
                ParseInstruction parseInstruction = parseInstructions[i];
                if (parameters.Length < i) {
                    ActionScriptInterpretorUtils.scriptError(scriptCommand,$"Ordered parameter {i} for {parseInstruction.Key} not present");
                }
                object parsed = parse(parseInstruction.ParseType, parameters[i],parseInstruction.Key,scriptCommand);
                objects.Add(parsed);
            }
            return objects;
        }

        public static Dictionary<string,object> parseDict(List<ParseInstruction> parseInstructions, string[] parameters, FormattedScriptCommand scriptCommand) {
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
                    ActionScriptInterpretorUtils.scriptError(scriptCommand, $"parameter {parameter} is formatted incorrectly. Format as parameter=val");
                }
                if (splitParameter.Length == 1) {
                    continue;
                    //ActionScriptInterpretorUtils.scriptError(scriptCommand, $"parameter {parameter} has no provided value. Format as parameter=val");
                }
                if (splitParameter.Length > 2) {
                    ActionScriptInterpretorUtils.scriptError(scriptCommand, $"parameter {parameter} has more than one '='");
                }
                string parameterName = splitParameter[0];
                string parameterVal = splitParameter[1];
                if (!parameterToInstruction.ContainsKey(parameterName)) {
                    continue;
                    //ActionScriptInterpretorUtils.scriptError(scriptCommand, $"parameter {parameterName} is not valid in this context");
                }
                if (requiredParameters.Contains(parameterName)) {
                    requiredParameters.Remove(parameterName);
                }
                if (usedParameters.Contains(parameterName)) {
                    ActionScriptInterpretorUtils.scriptError(scriptCommand,$"parameter {parameterName} is used multiple times");
                }
                usedParameters.Add(parameterName);
                dict[parameterName] = parse(parameterToInstruction[parameterName].ParseType,parameterVal,parameterName,scriptCommand);
            }
            return dict;
        }

        
            
    }

    public static class ActionScriptCommandParser {
        public static ActionTargetType parseCommandTargetType(FormattedScriptCommand scriptCommand) {
            Dictionary<string,object> parameters = ActionScriptParseUtils.parseDict(
                new List<ParseInstruction>{
                    new ParseInstruction(ParseType.Boolean,"self",false),
                },
                parameters: scriptCommand.Parameters,
                scriptCommand: scriptCommand
            );
            if (!parameters.ContainsKey("self")) {
                return ActionTargetType.Target;
            }
            bool targetSelf = (bool) parameters["self"];
            return targetSelf ? ActionTargetType.Self : ActionTargetType.Target;
        }

        public static ScriptedActionType getScriptedActionType(ScriptedAction scriptedAction) {
            Stack<ScriptCommand> commands = ScriptCommandFactory.parseCommands(scriptedAction.ActionScript);
            foreach (ScriptCommand scriptCommand in commands) {
                if (scriptCommand is SpecialSelectCommand specialSelectCommand) {
                    string val = specialSelectCommand.getFormattedScriptCommand().Parameters[0];
                    switch (val) {
                        case "target":
                            return ScriptedActionType.OnAttack;
                        case "attacker":
                            return ScriptedActionType.OnDamaged;
                        case "healer":
                            return ScriptedActionType.OnHealed;
                        default:
                            throw new Exception($"{val} is not a valid selectf parameter");
                    }
                }
            }
            return ScriptedActionType.Standard;
        }
    }

    public enum ParseType {
        Integer,
        Float,
        String,
        IntegerArray,
        Boolean
    }

    public class ParseInstruction {
        public ParseType ParseType;
        public string Key;
        public bool Required;
        public ParseInstruction(ParseType parseType, string key=null, bool required=false)
        {
            ParseType = parseType;
            Key = key;
            Required = required;
        }
    }


}
