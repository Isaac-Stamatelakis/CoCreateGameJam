using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Levels.Combat;

namespace Actions.Script {
    public class AttackCommand : InstantScriptCommand
    {
        public AttackCommand(FormattedScriptCommand formattedScriptCommand) : base(formattedScriptCommand)
        {
        }

        public override void execute(CommandExecutionState commandExecutionState)
        {
            (float damage, float range, DamageType damageType, float falloff, float lifesteal, bool self) = parse(formattedScriptCommand);
            CreatureCombatObject target = commandExecutionState.getActionTarget(self);
            float realDamage = UnityEngine.Random.Range(damage-range,damage+range);
            realDamage += falloff * commandExecutionState.SubStack.iterations;
            target.CreatureInCombat.hit(realDamage,damageType);
            commandExecutionState.SelfCreature.CreatureInCombat.heal(realDamage*lifesteal);
        }

        public static (float damage, float range, DamageType type, float falloff, float lifesteal, bool self) parse(FormattedScriptCommand scriptCommand) {
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
                    new ParseInstruction(ParseType.Boolean,
                    "self",
                    false)
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
            bool self = false;
            if (parameters.ContainsKey("self")) {
                self = (bool) parameters["self"];
            }
            return (damage,range,damageType,falloff,lifesteal,self);
        }
    }
}
