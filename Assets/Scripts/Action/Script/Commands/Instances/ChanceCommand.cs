using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Actions.Script {
    public class ChanceCommand : InstantScriptCommand
    {
        public ChanceCommand(FormattedScriptCommand formattedScriptCommand) : base(formattedScriptCommand)
        {
        }

        public override void execute(CommandExecutionState commandExecutionState)
        {
            (int numerator, int denominator) = parse(formattedScriptCommand);
            if (denominator == 0) {
                Debug.LogWarning("Chance command tried to divide by zero");
                endExecution(commandExecutionState);
                return;
            }
            float num = (float) numerator / denominator;
            System.Random random = new System.Random();
            float ran = (float)random.NextDouble();
            Debug.Log($"{num}|{ran}");
            if (ran >= num) {
                endExecution(commandExecutionState);
            }
        }

        private void endExecution(CommandExecutionState commandExecutionState) {
            if (commandExecutionState.SubStack == null) {
                commandExecutionState.Run = false;
            } else {
                commandExecutionState.SubStack.commands = new Stack<ScriptCommand>(); // Skips stack execution
            }
        }

        public static (int numerator, int denominator) parse(FormattedScriptCommand formattedScriptCommand) {
            List<object> parsedParameters = ActionScriptParseUtils.parseOrdered(
                new List<ParseInstruction>{
                    new ParseInstruction(ParseType.Integer,"Numerator",true),
                    new ParseInstruction(ParseType.Integer,"Denominator",true),
                },
                formattedScriptCommand.Parameters,
                formattedScriptCommand
            );
           
            int numerator = (int) parsedParameters[0];
            int denominator = (int) parsedParameters[1];
            return (numerator,denominator);
        }
    }
}

