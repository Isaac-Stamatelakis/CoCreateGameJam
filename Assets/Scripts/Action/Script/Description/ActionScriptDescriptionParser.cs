using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Actions;

namespace Actions.Script.Description {
    
    public class ActionScriptDescriptionParser
    {
        private Stack<ScriptCommand> commands;
        public ParseStage ParseStage;
        public Dictionary<ParseStage,List<ActionDictCollection>> ParseStageDictCollections;
        private bool inIfStatement;
        private string creatureName;
        private string description;
        private string preSelectDescription;
        private string selectDescription;
        private string postSelectDescription;
        public ActionScriptDescriptionParser(string actionScript, string creatureName, string preSelectDescription, string selectDescription, string postSelectDescription) {
            commands = ScriptCommandFactory.parseCommands(actionScript);
            ParseStage = ParseStage.PreSelect;
            ParseStageDictCollections = new Dictionary<ParseStage, List<ActionDictCollection>>();
            ParseStageDictCollections[ParseStage.PreSelect] = new List<ActionDictCollection>{
                new ActionDictCollection(null,null,new List<ActionTargetType>{ActionTargetType.Self},true)
            };
            ParseStageDictCollections[ParseStage.Selection] = new List<ActionDictCollection>();
            ParseStageDictCollections[ParseStage.PostSelect] = new List<ActionDictCollection>{
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
                if (scriptCommand is IDescriptableCommand descriptableCommand) {
                    descriptableCommand.execute(this);
                } else {
                    FormattedScriptCommand formattedScriptCommand = scriptCommand.getFormattedScriptCommand();
                    ActionTargetType actionTargetType = ActionScriptCommandParser.parseCommandTargetType(formattedScriptCommand);
                    ParseStageDictCollections[ParseStage].Last().addActionDescription(
                        formattedScriptCommand,
                        actionTargetType
                    );
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
                string actionDictDescription = actionDictCollection.getDescription(actionOrder);
                if (actionDictDescription == null || actionDictDescription.Length <= 1) {
                    continue;
                }
                description += $"{actionDictDescription} ";
            }
            return description;
        }

        private string formatDictCollection(ParseStage parseStage, string prefix, List<string> actionOrder) {
            if (ParseStageDictCollections[parseStage].Count == 0) {
                return "";
            }
            string dictCollectionDescription = listToDescription(ParseStageDictCollections[parseStage],actionOrder);
            return $"{prefix} {dictCollectionDescription}";
        }
    }
}

