using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions.Script.Description;
using System;
using Items.Equipment;

namespace Actions.Script {
    public class SpecialSelectCommand : InstantScriptCommand, IDescriptableCommand
    {
        public SpecialSelectCommand(FormattedScriptCommand formattedScriptCommand) : base(formattedScriptCommand)
        {
        }

        public override void execute(CommandExecutionState commandExecutionState)
        {
            commandExecutionState.CreatureSelector = new CreatureSelector(
                targetType: CreatureSelectionType.Any,
                allowSelf: false,
                maxTargets: 1
            );
            //commandExecutionState.PausedForSelection=true;
            CommandExecutionStateUtils.generateSubStack(commandExecutionState);
        }

        public void execute(ActionScriptDescriptionParser actionScriptDescriptionParser)
        {
            if (actionScriptDescriptionParser.ParseStage == ParseStage.PreSelect) {
                actionScriptDescriptionParser.ParseStage = ParseStage.Selection;
            } else {
                throw new Exception("'select' called multiple times");
            }
            actionScriptDescriptionParser.ParseStageDictCollections[actionScriptDescriptionParser.ParseStage].Add(new ActionDictCollection(
                prefix: null,
                suffix: getDescription(),
                ParseStageUtils.getTargetTypes(actionScriptDescriptionParser.ParseStage),
                true
            ));
        }

        private string getDescription() {
            SpecialSelectTarget specialSelectTarget = SpecialSelectCommand.parse(formattedScriptCommand);
            switch (specialSelectTarget) {
                case SpecialSelectTarget.Attacker:
                    return  $"to their {specialSelectTarget}";
                case SpecialSelectTarget.Healer:
                    return $"to their {specialSelectTarget}";
                case SpecialSelectTarget.Target:
                    return "on hit";

            }
            return $"to their {specialSelectTarget}";
        }

        public static SpecialSelectTarget parse(FormattedScriptCommand scriptCommand) {
            return GlobalUtils.stringToEnum<SpecialSelectTarget>(scriptCommand.Parameters[0]);
        }
    }
    public enum SpecialSelectTarget {
        Attacker,
        Target,
        Healer,
    }

    public static class SpecialSelectCommandUtils {
        public static ScriptedAction getAction(EquipmentActionCollection equipmentActionCollection, SpecialSelectTarget specialSelectTarget) {
            switch (specialSelectTarget) {
                case SpecialSelectTarget.Attacker:
                    return equipmentActionCollection.onDamaged;
                case SpecialSelectTarget.Target:
                    return equipmentActionCollection.onAttack;
                case SpecialSelectTarget.Healer:
                    return equipmentActionCollection.onHealed;
                default:
                    throw new Exception($"Did not cover case for {specialSelectTarget}");
            }
        }
    }
}

