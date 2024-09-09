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

        public override void execute(LiveCommandExecutionState commandExecutionState)
        {
            (int targets, CreatureSelectionType targetType, bool random, bool targetSelf, float? period) = parse(formattedScriptCommand);
            commandExecutionState.SelectionPeriod = period;
            if (random) {
                List<CreatureCombatObject> randomCreatures = new List<CreatureCombatObject>();
                GameState gameState = CombatLevelController.Instance.GameState;
                for (int i = 0; i < targets; i++) {
                    CreatureInCombat randomCombatCreature = gameState.getRandomCreature(targetType,targetSelf);
                    randomCreatures.Add(randomCombatCreature.CreatureCombatObject);
                }
                commandExecutionState.CreatureSelector = new DescriptableRandomSelector(randomCreatures,targetType,targetSelf,targets);
            } else {
                commandExecutionState.CreatureSelector = new ManualCreatureSelector(
                    targetType: targetType,
                    allowSelf: targetSelf,
                    maxTargets: targets
                );
                commandExecutionState.PausedForSelection = true;
            }
            CommandExecutionStateUtils.generateSubStack(commandExecutionState);
            
        }
        public static (int targets, CreatureSelectionType targetType, bool random, bool targetSelf, float? period) parse(FormattedScriptCommand scriptCommand) {
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
                    new ParseInstruction(ParseType.Boolean,"self",false),
                    new ParseInstruction(ParseType.Float,"period",false)
                },
                parameters: scriptCommand.Parameters,
                scriptCommand: scriptCommand
            );
            bool random = false;
            bool targetSelf = true;
            float? period = null;
            if (dictParameters.ContainsKey("random")) {
                random = (bool) dictParameters["random"];
            }
            if (dictParameters.ContainsKey("self")) {
                targetSelf = (bool) dictParameters["self"];
            }
            if (dictParameters.ContainsKey("period")) {
                period = (float) dictParameters["period"];
            }
            return (targets,targetType,random,targetSelf,period);
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
            (int targets, CreatureSelectionType type, bool random, bool targetSelf, float? period) = SelectCommand.parse(formattedScriptCommand);
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