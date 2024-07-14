using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions.Script {
    public class HealCommand : InstantScriptCommand
    {
        public HealCommand(FormattedScriptCommand formattedScriptCommand) : base(formattedScriptCommand)
        {
        }

        public override void execute(CommandExecutionState commandExecutionState)
        {
            (float amount, float range, bool self) = parse(formattedScriptCommand);
            float healAmount = Random.Range(amount-range,amount+range);
            if (self) {
                commandExecutionState.SelfCreature.CreatureInCombat.heal(healAmount);
            } else {
                commandExecutionState.TargetCreature.CreatureInCombat.heal(healAmount);
            }
        }

        public static (float amount, float range, bool self) parse(FormattedScriptCommand scriptCommand) {
            List<object> orderedParameters = ActionScriptParseUtils.parseOrdered(
            parseInstructions: new List<ParseInstruction>{
                    new ParseInstruction(ParseType.Float,"heal",true),
                },
                parameters: scriptCommand.Parameters,
                scriptCommand: scriptCommand
            );
            
            Dictionary<string,object> parameters = ActionScriptParseUtils.parseDict(
                new List<ParseInstruction>{
                    new ParseInstruction(ParseType.Float,"range",false),
                    new ParseInstruction(ParseType.Boolean,"self",false)
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
            float heal = (float) orderedParameters[0];
            return (heal,range,self);
        }
    }
}

