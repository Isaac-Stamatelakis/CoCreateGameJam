using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Levels.Combat;
using System;
using Actions.Script.Description;
using Actions.Script.Execution;

namespace Actions.Script {
    public class IfCommand : InstantScriptCommand, IDescriptableCommand, ISimultableCommand
    {
        public IfCommand(FormattedScriptCommand formattedScriptCommand) : base(formattedScriptCommand)
        {
        }

        public static (string first, string booleanOperator, string second) parse(FormattedScriptCommand scriptCommand) {
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
        public override void execute(LiveCommandExecutionState commandExecutionState)
        {
            executeState(commandExecutionState);
        }

        private void executeState(ICommandExecutionState commandExecutionState) {
            (string first, string booleanOperator, string second) = parse(formattedScriptCommand);
            bool statementPassed = false;
            if (booleanOperator.Equals("has")) {
                string creatureIndicator = first;
                string statusIndicator = second;
                ICombatCreature creatureInCombat = commandExecutionState.getCreatureFromIndicator(formattedScriptCommand,creatureIndicator);
                statementPassed = creatureInCombat.hasStatus(statusIndicator);
            } else if (
                booleanOperator.Equals("<")  || 
                booleanOperator.Equals(">")  || 
                booleanOperator.Equals("<=") || 
                booleanOperator.Equals(">=") || 
                booleanOperator.Equals("==")
            ) {
                double a = ActionScriptParseUtils.parseDoubleValue(first,formattedScriptCommand,commandExecutionState);
                double b = ActionScriptParseUtils.parseDoubleValue(second,formattedScriptCommand,commandExecutionState);
                switch (booleanOperator) {
                    case "<":
                        statementPassed = a < b;
                        break;
                    case ">":
                        statementPassed = a > b;
                        break;
                    case "<=":
                        statementPassed = a <= b;
                        break;
                    case ">=":
                        statementPassed = a >= b;
                        break;
                    case "==":
                        statementPassed = a == b;
                        break;
                }
            } else if (booleanOperator.Equals("is")) {
                string creatureIndicator = first;
                string statusIndicator = second;
                ICombatCreature creatureInCombat = commandExecutionState.getCreatureFromIndicator(formattedScriptCommand,creatureIndicator);
            } else {
                ActionScriptInterpretorUtils.scriptError(formattedScriptCommand,$"{booleanOperator} is not a valid boolean operator");
            }
            if (statementPassed) {
                return;
            }
            if (commandExecutionState.getSubStack() != null) {
                skipIfStatement(formattedScriptCommand,commandExecutionState.getSubStack().commands);
            } else {
                skipIfStatement(formattedScriptCommand,commandExecutionState.getStack());
            }
        }

        public void execute(ActionScriptDescriptionParser actionScriptDescriptionParser)
        {
            actionScriptDescriptionParser.ParseStageDictCollections[actionScriptDescriptionParser.ParseStage].Add(new ActionDictCollection(
                prefix: "Then ",
                suffix: getIfDescription(formattedScriptCommand),
                ParseStageUtils.getTargetTypes(actionScriptDescriptionParser.ParseStage),
                true
            ));
        }

        private string getIfDescription(FormattedScriptCommand scriptCommand) {
            (string first, string booleanOperator, string second) = IfCommand.parse(scriptCommand);
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

        private void skipIfStatement(FormattedScriptCommand ifCommand, Stack<ScriptCommand> commands) {
            while (commands.Count > 0) {
                ScriptCommand scriptCommand = commands.Pop();
                if (scriptCommand is not EndCommand endCommand) {
                    continue;
                }
                if (endCommand.endsIf()) {
                    return;
                }
            }
            // Only gets here if commands are emptied ie no 'end if' statement was present in script
            ActionScriptInterpretorUtils.scriptError(ifCommand,"'if' was called with no 'end if'");
        }

        public void execute(SimulatedExecutionState state)
        {
            executeState(state);
        }
    }

}
