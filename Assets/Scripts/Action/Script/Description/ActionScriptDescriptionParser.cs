using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Actions.Script {
    
    public class ActionScriptDescriptionParser
    {
        private Stack<ScriptCommand> commands;
        private ParseStage parseStage;
        private Dictionary<ParseStage,List<ActionDictCollection>> parseStageDictCollections;
        private bool inIfStatement;
        private string creatureName;
        private string description;
        private string preSelectDescription;
        private string selectDescription;
        private string postSelectDescription;
        public ActionScriptDescriptionParser(string actionScript, string creatureName, string preSelectDescription, string selectDescription, string postSelectDescription) {
            commands = ActionScriptParseUtils.parseCommands(actionScript);
            parseStage = ParseStage.PreSelect;
            parseStageDictCollections = new Dictionary<ParseStage, List<ActionDictCollection>>();
            parseStageDictCollections[ParseStage.PreSelect] = new List<ActionDictCollection>{
                new ActionDictCollection(null,null,new List<ActionTargetType>{ActionTargetType.Self},true)
            };
            parseStageDictCollections[ParseStage.Selection] = new List<ActionDictCollection>();
            parseStageDictCollections[ParseStage.PostSelect] = new List<ActionDictCollection>{
                new ActionDictCollection(null,null,new List<ActionTargetType>{ActionTargetType.Self},true)
            };
            this.preSelectDescription = preSelectDescription;
            this.selectDescription = selectDescription;
            this.postSelectDescription = postSelectDescription;
            this.creatureName = creatureName;
        }
        public string getDescription() {
            if (description != null) {
                return description;
            }
            while (commands.Count > 0) {
                ScriptCommand scriptCommand = commands.Pop();
                switch (scriptCommand.Command) {
                    case "select":
                        if (parseStage == ParseStage.PreSelect) {
                            parseStage = ParseStage.Selection;
                        } else {
                            throw new Exception("'select' called multiple times");
                        }
                        parseStageDictCollections[parseStage].Add(new ActionDictCollection(
                            prefix: getSelectDescription(scriptCommand),
                            suffix: null,
                            ParseStageUtils.getTargetTypes(parseStage),
                            true
                        ));
                        break;
                    case "if":
                        parseStageDictCollections[parseStage].Add(new ActionDictCollection(
                            prefix: null,
                            suffix: getIfDescription(scriptCommand),
                            ParseStageUtils.getTargetTypes(parseStage),
                            true
                        ));
                        break;
                    case "end":
                        if (scriptCommand.Parameters.Length == 0) {
                            break;
                        }
                        if (scriptCommand.Parameters[0].Equals("if")) {
                            parseStageDictCollections[parseStage].Add(new ActionDictCollection(
                                prefix: "Otherwise, ",
                                suffix: null,
                                ParseStageUtils.getTargetTypes(parseStage),
                                false
                            ));
                        }
                        if (scriptCommand.Parameters[0].Equals("select")) {
                            if (parseStage != ParseStage.Selection) {
                                throw new Exception("'end select' called prior to select");
                            }
                            parseStage = ParseStage.PostSelect;
                        }
                        break;
                    default:
                        ActionTargetType actionTargetType = ActionScriptCommandParser.parseCommandTargetType(scriptCommand);
                        parseStageDictCollections[parseStage].Last().addActionDescription(
                            scriptCommand,
                            actionTargetType
                        );
                        break;
                }
            }
            List<string> actionOrder = ScriptDescriptionCollectionUtils.getActionOrder();
            description += formatDictCollection(ParseStage.PreSelect,preSelectDescription,actionOrder);
            description += formatDictCollection(ParseStage.Selection,selectDescription,actionOrder);
            description += formatDictCollection(ParseStage.PostSelect,postSelectDescription,actionOrder);
            return description.Substring(0, description.Length - 1); // Remove empty space at end
        }

        private string listToDescription(List<ActionDictCollection> actionDictCollections, List<string> actionOrder) {
            string description = "";
            for (int i = 0; i < actionDictCollections.Count; i++) {
                ActionDictCollection actionDictCollection = actionDictCollections[i];
                description += $"{actionDictCollection.getDescription(actionOrder)} ";
            }
            foreach (ActionDictCollection actionDictCollection in actionDictCollections) {
                
            }
            return description;
        }

        private string formatDictCollection(ParseStage parseStage, string prefix, List<string> actionOrder) {
            if (parseStageDictCollections[parseStage].Count == 0) {
                return "";
            }
            string dictCollectionDescription = listToDescription(parseStageDictCollections[parseStage],actionOrder);
            if (prefix == null || prefix.Length == 0) {
                prefix = "{EMPTY DESCRIPTION}";
            }
            return $"{prefix} {dictCollectionDescription}";
        }

        private string getSelectDescription(ScriptCommand scriptCommand) {
            (int targets, string type, bool random, bool targetSelf) = ActionScriptCommandParser.parseSelectCommand(scriptCommand);
            string targetString = ScriptDescriptionCollectionUtils.integerToText(targets);
            string selectString = targetString;
            if (random) {
                selectString += " random";
            }
            selectString += $" {type}";
            if (!targetSelf) {
                selectString += " except them";
            }
            return $"{selectString} ";
        }

        private string getIfDescription(ScriptCommand scriptCommand) {
            (string first, string booleanOperator, string second) = ActionScriptCommandParser.parseIfCommand(scriptCommand);
            if (booleanOperator.Equals("has")) {
                string creatureIndicator = first;
                string statusIndicator = second;
                
            } else if (
                booleanOperator.Equals("<")  || 
                booleanOperator.Equals(">")  || 
                booleanOperator.Equals("<=") || 
                booleanOperator.Equals(">=") || 
                booleanOperator.Equals("==")
            ) {
                
                (string formatedFirst, bool firstFloat) = getComparedIfValue(first);
                (string formatedSecond, bool secondFloat) = getComparedIfValue(second);
                string booleanOperatorEnglish = null;
                switch (booleanOperator) {
                    case "<":
                        booleanOperatorEnglish = "less than";
                        break;
                    case ">":
                        booleanOperatorEnglish = "greater than";
                        break;
                    case "<=":
                        booleanOperatorEnglish = "less than or equal to";
                        break;
                    case ">=":
                        booleanOperatorEnglish = "greater than or equal to";
                        break;
                    case "==":
                        booleanOperatorEnglish = "equal to";
                        break;
                }
                if (secondFloat) {
                    return $"if {formatedFirst} is {booleanOperatorEnglish} {formatedSecond}";
                } else {
                    return $"if {formatedFirst} is {booleanOperatorEnglish} their {formatedSecond}";
                }
                

            } else if (booleanOperator.Equals("is")) {
                string creatureIndicator = first;
                string statusIndicator = second;   
            }
            return null;
        }

        private (string,bool) getComparedIfValue(string val) {
            bool isFloat = float.TryParse(val, out float floatVal);
            if (isFloat) {
                return ($"{floatVal : F1}",true);
            }
            string[] split = val.Split(".");
            string creatureIndicator = split[0];
            string attributeIndicator = split[1];

            string creatureName = null;
            switch (creatureIndicator) {
                case "self":
                    creatureName = "their";
                    break;
                case "target":
                    creatureName = "their target's";
                    break;
                default:
                    throw new Exception($"{creatureIndicator} is not a valid creature indiciator");
            }
            string attributeName = attributeIndicator.Replace("_","");
            return ($"{creatureName} {attributeName}",false);
        }
        private class ActionDictCollection {
            private string prefix;
            private string suffix;
            private bool passiveVoice;
            private List<ActionTargetType> validTargets;
            private List<ActionDescriptionCollectionDict> actionDescriptionCollections;
            public ActionDictCollection(string prefix, string suffix, List<ActionTargetType> validTargets, bool passiveVoice)
            {
                actionDescriptionCollections = new List<ActionDescriptionCollectionDict>();
                this.validTargets = validTargets;
                this.prefix = prefix;
                this.suffix = suffix;
                this.passiveVoice = passiveVoice;
            }
            public void addActionDescription(ScriptCommand scriptCommand, ActionTargetType targetType) {
                if (!validTargets.Contains(targetType)) {
                    throw new Exception($"{targetType} is not a valid target in this collection");
                }
                ActionDescriptionCollectionDict actionDescriptionCollection = null;
                if (actionDescriptionCollections.Count == 0 || !actionDescriptionCollections.Last().isTargetType(targetType)) {
                    actionDescriptionCollection = new ActionDescriptionCollectionDict(targetType);
                    actionDescriptionCollections.Add(actionDescriptionCollection);
                } else {
                    actionDescriptionCollection = actionDescriptionCollections.Last();
                }
                actionDescriptionCollection.addCommand(scriptCommand);
            }

            public string getDescription(List<string> actionOrder) {
                if (actionDescriptionCollections.Count == 0) {
                    return null;
                }
                string description = $"{prefix}";
                for (int i = 0; i < actionDescriptionCollections.Count; i++) {
                    ActionDescriptionCollectionDict collectionDict = actionDescriptionCollections[i];
                    string actionDescritionCollection = collectionDict.getDescription(actionOrder,passiveVoice);
                    if (actionDescritionCollection == null) {
                        continue;
                    }
                    bool isLast = i == actionDescriptionCollections.Count-1;
                    if (isLast) {
                        description += actionDescritionCollection;
                    } else {
                        description += $"{actionDescritionCollection}.";
                    }
                }
                if (suffix == null) {
                    return $"{description}.";
                } else {
                    return $"{description} {suffix}.";
                }
            }
        }

        private enum ParseStage {
            PreSelect,
            Selection,
            PostSelect
        }

        private static class ParseStageUtils {
            public static List<ActionTargetType> getTargetTypes(ParseStage parseStage) {
                switch (parseStage) {
                    case ParseStage.PreSelect:
                        return new List<ActionTargetType>{
                            ActionTargetType.Self
                        };
                    case ParseStage.Selection:
                        return new List<ActionTargetType>{
                            ActionTargetType.Self,
                            ActionTargetType.Target
                        };
                    case ParseStage.PostSelect:
                        return new List<ActionTargetType>{
                            ActionTargetType.Self
                        };
                    default:
                        throw new Exception($"{parseStage} was not covered by 'getTargetTypes'");
                }
            }
        }
    }
    public enum ActionTargetType {
        Self,
        Target
    }
}

