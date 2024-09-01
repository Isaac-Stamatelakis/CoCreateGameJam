using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions;
using Actions.Script.Execution;
using Creatures;
using System;
using Actions.Script.Description;
using Levels.Combat;

namespace Actions.Script {


    public class SelectCommand : InstantScriptCommand, IDescriptableCommand
    {
        public SelectCommand(FormattedScriptCommand formattedScriptCommand) : base(formattedScriptCommand)
        {
        }

        public override void execute(CommandExecutionState commandExecutionState)
        {
            (int targets, CreatureSelectionType targetType, bool random, bool targetSelf) = parse(formattedScriptCommand);
            commandExecutionState.CreatureSelector = new CreatureSelector(
                targetType: targetType,
                allowSelf: targetSelf,
                maxTargets: targets,
                random: random
            );
            if (random) {
                for (int i = 0; i < targets; i++) {
                    commandExecutionState.CreatureSelector.Creatures.Add(
                        commandExecutionState.CombatLevelController.getRandomCreature(targetSelf).CreatureCombatObject
                    );
                }
            } else {
                commandExecutionState.PausedForSelection = true;
            }
            CommandExecutionStateUtils.generateSubStack(commandExecutionState);
            
        }
        public static (int targets, CreatureSelectionType targetType, bool random, bool targetSelf) parse(FormattedScriptCommand scriptCommand) {
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
            CreatureSelectionType targetType = GlobalUtils.stringToEnum<CreatureSelectionType>(type);
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
            return (targets,targetType,random,targetSelf);
        }

        public void execute(ActionScriptDescriptionParser actionScriptDescriptionParser)
        {
            if (actionScriptDescriptionParser.ParseStage == ParseStage.PreSelect) {
                actionScriptDescriptionParser.ParseStage = ParseStage.Selection;
            } else {
                throw new Exception("'select' called multiple times");
            }
            actionScriptDescriptionParser.ParseStageDictCollections[actionScriptDescriptionParser.ParseStage].Add(new ActionDictCollection(
                prefix: getDescription(),
                suffix: null,
                ParseStageUtils.getTargetTypes(actionScriptDescriptionParser.ParseStage),
                true
            ));
        }

        private string getDescription() {
            (int targets, CreatureSelectionType type, bool random, bool targetSelf) = SelectCommand.parse(formattedScriptCommand);
            string targetString = ScriptDescriptionCollectionUtils.integerToText(targets);
            string selectString = targetString;
            if (random) {
                selectString += " random";
            }
            string targetTypeString = type.ToString().ToLower();
            selectString += $" {targetTypeString}";
            if (!targetSelf) {
                selectString += " except them";
            }
            return $"{selectString} ";
        }
    }
}