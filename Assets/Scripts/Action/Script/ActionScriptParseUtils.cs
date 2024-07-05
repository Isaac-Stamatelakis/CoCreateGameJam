using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Creatures;
using Items;

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

        public static double parseCreatureAttribute(ScriptCommand scriptCommand, string attribute, CreatureInCombat creatureInCombat) {
            switch (attribute) {
                case "health":
                    return creatureInCombat.Health;
                case "health_percent":
                    return creatureInCombat.HealthPercent;
                case "mana":
                    return creatureInCombat.Mana;
                case "mana_percent":
                    return creatureInCombat.ManaPercent;
                case "speed":
                    return creatureInCombat.EquipedCreeture.getStat(CreatureStat.Speed);
                case "attack":
                    return creatureInCombat.EquipedCreeture.getStat(CreatureStat.Attack);
                case "defense":
                    return creatureInCombat.EquipedCreeture.getStat(CreatureStat.Armor);
                case "ability_power":   
                    return creatureInCombat.EquipedCreeture.getStat(CreatureStat.Ability);
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
        public static object parse(ParseType parseType, string val, string parameterDescription, ScriptCommand scriptCommand) {
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

        public static List<object> parseOrdered(List<ParseInstruction> parseInstructions, string[] parameters, ScriptCommand scriptCommand) {
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

        public static Dictionary<string,object> parseDict(List<ParseInstruction> parseInstructions, string[] parameters, ScriptCommand scriptCommand) {
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
        public static ActionTargetType parseCommandTargetType(ScriptCommand scriptCommand) {
            Dictionary<string,object> parameters = ActionScriptParseUtils.parseDict(
                new List<ParseInstruction>{
                    new ParseInstruction(ParseType.Boolean,"target_self",false),
                },
                parameters: scriptCommand.Parameters,
                scriptCommand: scriptCommand
            );
            if (!parameters.ContainsKey("target_self")) {
                return ActionTargetType.Target;
            }
            bool targetSelf = (bool) parameters["target_self"];
            return targetSelf ? ActionTargetType.Self : ActionTargetType.Target;
        }
        public static (int targets, string type, bool random, bool targetSelf) parseSelectCommand(ScriptCommand scriptCommand) {
            List<object> orderedParameters = ActionScriptParseUtils.parseOrdered(
            parseInstructions: new List<ParseInstruction>{
                    new ParseInstruction(ParseType.Integer,"targets",true),
                    new ParseInstruction(ParseType.String,"type",true)
                },
                parameters: scriptCommand.Parameters,
                scriptCommand: scriptCommand
            );
            int targets = (int) orderedParameters[0];
            string type = (string) orderedParameters[1];
            Dictionary<string,object> dictParameters = ActionScriptParseUtils.parseDict(
                new List<ParseInstruction>{
                    new ParseInstruction(ParseType.Boolean,"random",false),
                    new ParseInstruction(ParseType.Boolean,"self",false)
                },
                parameters: scriptCommand.Parameters,
                scriptCommand: scriptCommand
            );
            bool random = false;
            bool targetSelf = true;
            if (dictParameters.ContainsKey("random")) {
                random = (bool) dictParameters["random"];
            }
            if (dictParameters.ContainsKey("self")) {
                targetSelf = (bool) dictParameters["self"];
            }
            return (targets,type,random,targetSelf);
        }

        public static (float damage, float range, DamageType type, float falloff, float lifesteal) parseAttackCommand(ScriptCommand scriptCommand) {
            List<object> orderedParameters = ActionScriptParseUtils.parseOrdered(
            parseInstructions: new List<ParseInstruction>{
                    new ParseInstruction(ParseType.Float,"damage",true),
                },
                parameters: scriptCommand.Parameters,
                scriptCommand: scriptCommand
            );
            
            Dictionary<string,object> parameters = ActionScriptParseUtils.parseDict(
                new List<ParseInstruction>{
                    new ParseInstruction(ParseType.Float,
                    "range",
                    false
                    ),
                    new ParseInstruction(ParseType.String,
                    "type",
                    false
                    ),
                    new ParseInstruction(ParseType.Float,
                    "falloff",
                    false
                    ),
                    new ParseInstruction(ParseType.Float,
                    "lifesteal",
                    false
                    ),
                },
                scriptCommand.Parameters,
                scriptCommand
            );
            float range = 0;
            if (parameters.ContainsKey("range")) {
                range = (float) parameters["range"];
            }
            DamageType damageType = DamageType.Physical;
            if (parameters.ContainsKey("type")) {
                string damageString = (string) parameters["type"];
                damageType = GlobalUtils.stringToEnum<DamageType>(damageString);
            }
            float falloff = 0;
            if (parameters.ContainsKey("falloff")) {
                falloff = (float) parameters["falloff"];
            }
            float lifesteal = 0;
            if (parameters.ContainsKey("lifesteal")) {
                lifesteal = (float) parameters["lifesteal"];
            }
            float damage = (float) orderedParameters[0];
            return (damage,range,damageType,falloff,lifesteal);
        }

        public static (string first, string booleanOperator, string second) parseIfCommand(ScriptCommand scriptCommand) {
            List<object> parsedParameters = ActionScriptParseUtils.parseOrdered(
                new List<ParseInstruction>{
                    new ParseInstruction(ParseType.String,"First Value",true),
                    new ParseInstruction(ParseType.String,"Comparitor",true),
                    new ParseInstruction(ParseType.String,"Second Value",true)
                },
                scriptCommand.Parameters,
                scriptCommand
            );
            return ((string) parsedParameters[0],(string) parsedParameters[1],(string) parsedParameters[2]);
        }
        public static (float damage, float range) parseHealCommand(ScriptCommand scriptCommand) {
            List<object> orderedParameters = ActionScriptParseUtils.parseOrdered(
            parseInstructions: new List<ParseInstruction>{
                    new ParseInstruction(ParseType.Float,"heal",true),
                },
                parameters: scriptCommand.Parameters,
                scriptCommand: scriptCommand
            );
            
            Dictionary<string,object> parameters = ActionScriptParseUtils.parseDict(
                new List<ParseInstruction>{
                    new ParseInstruction(ParseType.Float,
                    "range",
                    false
                    ),
                },
                scriptCommand.Parameters,
                scriptCommand
            );
            float range = 0;
            if (parameters.ContainsKey("range")) {
                range = (float) parameters["range"];
            }
            float heal = (float) orderedParameters[0];
            return (heal,range);
        }

        public static (float amount, float range, float steal, bool percent, bool currentMana) parseManaCommand(ScriptCommand scriptCommand) {
            List<object> orderedParameters = ActionScriptParseUtils.parseOrdered(
            parseInstructions: new List<ParseInstruction>{
                    new ParseInstruction(ParseType.Integer,"amount",true)
                },
                parameters: scriptCommand.Parameters,
                scriptCommand: scriptCommand
            );
            
            Dictionary<string,object> parameters = ActionScriptParseUtils.parseDict(
                new List<ParseInstruction>{
                    new ParseInstruction(ParseType.Float,"range",false),
                    new ParseInstruction(ParseType.Float,"steal",false),
                    new ParseInstruction(ParseType.Boolean,"percent",false),
                    new ParseInstruction(ParseType.Boolean,"current",false)
                },
                scriptCommand.Parameters,
                scriptCommand
            );
            float range = 0;
            if (parameters.ContainsKey("range")) {
                range = (float) parameters["range"];
            }
            float amount = (float) orderedParameters[0];
            float steal = 0;
            if (parameters.ContainsKey("steal")) {
                steal = (float) parameters["steal"];
            }
            bool percent = false;
            if (parameters.ContainsKey("percent")) {
                percent = (bool) parameters["percent"];
            }
            bool currentMana = false;
            if (parameters.ContainsKey("current")) {
                currentMana = (bool) parameters["current"];
            }
            return (amount,range,steal,percent,currentMana);
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
