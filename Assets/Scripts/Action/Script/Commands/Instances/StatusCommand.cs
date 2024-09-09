using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions.Script {
    public class StatusCommand : InstantScriptCommand
    {
        public StatusCommand(FormattedScriptCommand formattedScriptCommand) : base(formattedScriptCommand)
        {
        }

        public override void execute(LiveCommandExecutionState commandExecutionState)
        {
            (StatusSubCommand subCommand, string[] subParameters) = parse(formattedScriptCommand);
            switch (subCommand) {
                case StatusSubCommand.add:
                    executeAdd(commandExecutionState,subParameters);
                    break;
                case StatusSubCommand.cleanse:
                    executeCleanse(commandExecutionState,subParameters);
                    break;
                default:
                    throw new System.Exception($"{subCommand} was not covered by Status execution");
            }
        }
        private void executeAdd(LiveCommandExecutionState commandExecutionState, string[] subParameters) {
            
        }
        private void executeCleanse(LiveCommandExecutionState commandExecutionState, string[] subParameters) {

        }

        public static (StatusSubCommand subCommand, string[] subParameters) parse(FormattedScriptCommand formattedScriptCommand) {
            List<object> orderedParameters = ActionScriptParseUtils.parseOrdered(
            parseInstructions: new List<ParseInstruction>{
                    new ParseInstruction(ParseType.String,"SubCommand",true),
                },
                parameters: formattedScriptCommand.Parameters,
                scriptCommand: formattedScriptCommand
            );
            StatusSubCommand statusSubCommand = GlobalUtils.stringToEnum<StatusSubCommand>((string)orderedParameters[0]);
            string[] subParameters = ActionScriptParseUtils.reduceArraySize<string>(formattedScriptCommand.Parameters,1);
            return (statusSubCommand,subParameters);
        }

        
    }

    public enum StatusSubCommand {
        add,
        cleanse
    }
}

