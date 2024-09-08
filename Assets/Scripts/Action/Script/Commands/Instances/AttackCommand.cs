using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Levels.Combat;
using Items.Equipment;
using Actions.Script.Execution;
using Creatures;

namespace Actions.Script {
    public class AttackCommand : DelayScriptCommand, ISimultableCommand
    {
        public AttackCommand(FormattedScriptCommand formattedScriptCommand) : base(formattedScriptCommand)
        {
        }

        private void executeState(ICommandExecutionState commandExecutionState, float effectiveness = 1) {
            (float damage, float range, DamageType damageType, float falloff, float lifesteal, bool self) = parse(formattedScriptCommand);
            ICombatCreature target = commandExecutionState.getActionTarget(self);
            if (target==null) {
                return;
            }
            float realDamage = UnityEngine.Random.Range(damage-range,damage+range);
            realDamage += falloff * commandExecutionState.getSubStack().iterations;
            target.hit(realDamage,damageType);
            EquipmentActionExecutor equipmentActionExecutor = commandExecutionState.getEquipmentActionExecutor();
            /*
            if (equipmentActionExecutor != null) {
                equipmentActionExecutor.setAttack(commandExecutionState.getSelfCreature(),target);
                if (realDamage*lifesteal > 0) {
                    equipmentActionExecutor.setHeal(commandExecutionState.getSelfCreature(),commandExecutionState.getSelfCreature());
                }
            }
            */
            commandExecutionState.getSelfCreature().heal(realDamage*lifesteal);
        }
        public override IEnumerator execute(LiveCommandExecutionState commandExecutionState)
        {
            executeState((ICommandExecutionState)commandExecutionState);
            yield return new WaitForSeconds(0.1f);
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

        public void execute(SimulatedExecutionState state)
        {
            executeState(state,state.getActionEffectiveness());
        }
    }
}
