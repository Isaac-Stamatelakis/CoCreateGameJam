using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions.Script {
    public class ManaCommand : InstantScriptCommand
    {
        public ManaCommand(FormattedScriptCommand formattedScriptCommand) : base(formattedScriptCommand)
        {
            
        }

        public override void execute(CommandExecutionState commandExecutionState)
        {
            
        }

        public static (ManaSubCommand subCommand, float amount, ManaParamType paramType, float range, bool self) parse(FormattedScriptCommand scriptCommand) {
            List<object> orderedParameters = ActionScriptParseUtils.parseOrdered(
            parseInstructions: new List<ParseInstruction>{
                    new ParseInstruction(ParseType.String,"type",true),
                    new ParseInstruction(ParseType.Integer,"amount",true)
                },
                parameters: scriptCommand.Parameters,
                scriptCommand: scriptCommand
            );
            ManaSubCommand subCommand = GlobalUtils.stringToEnum<ManaSubCommand>((string) orderedParameters[0]);
            float amount = (float) orderedParameters[1];
            Dictionary<string,object> parameters = ActionScriptParseUtils.parseDict(
                new List<ParseInstruction>{
                    new ParseInstruction(ParseType.Float,"range",false),
                    new ParseInstruction(ParseType.String,"modify",false)
                },
                scriptCommand.Parameters,
                scriptCommand
            );
            float range = 0;
            if (parameters.ContainsKey("range")) {
                range = (float) parameters["range"];
            }
            bool self = false;
            if (parameters.ContainsKey("self")) {
                self = (bool) parameters["self"];
            }
            ManaParamType manaParamType = ManaParamType.num;
            if (parameters.ContainsKey("type")) {
                manaParamType = GlobalUtils.stringToEnum<ManaParamType>((string) parameters["type"]);
            }
            return (subCommand, amount,manaParamType,range,self);
        }

        
    }

    public enum ManaSubCommand {
        add,
        remove,
        steal
    }

    public enum ManaParamType {
        num,
        max,
        current
    }
}

