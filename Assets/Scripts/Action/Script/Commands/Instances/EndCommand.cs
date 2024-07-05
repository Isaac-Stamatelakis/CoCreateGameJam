using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions.Script.Description;
using System;

namespace Actions.Script {
    public class EndCommand : InstantScriptCommand, IDescriptableCommand
    {
        public EndCommand(FormattedScriptCommand formattedScriptCommand) : base(formattedScriptCommand)
        {

        }

        public override void execute(CommandExecutionState commandExecutionState)
        {
            if (formattedScriptCommand.Parameters.Length == 0) {
                ActionScriptInterpretorUtils.scriptError(formattedScriptCommand,"'end' must be called with atleast one parameter");
            }
            string subCommand = formattedScriptCommand.Parameters[0];
            switch (subCommand) {
                case "select":
                    // 'end select' has no execution behavior. It is skipped past if an if statement fails.
                    break;
                case "spawn":
                    endSpawn(commandExecutionState);
                    break;
                case "if":
                    // 'end if' has no execution behavior. It is skipped past if an if statement fails.
                    break;
                default:
                    ActionScriptInterpretorUtils.scriptError(formattedScriptCommand,$"{subCommand} is not a valid end command");
                    break;
            }
        }
        public bool endsSelect() {
            return endsStatement("select");
        }
        public bool endsIf() {
            return endsStatement("if");
        }

        public bool endsStatement(string statement) {
            if (formattedScriptCommand.Parameters.Length == 0) {
                ActionScriptInterpretorUtils.scriptError(formattedScriptCommand,"'end' must be called with atleast one parameter");
            }
            return formattedScriptCommand.Parameters[0].Equals(statement);
        }
        private void endSpawn(CommandExecutionState commandExecutionState) {
            if (formattedScriptCommand.Parameters.Length < 2) {
                ActionScriptInterpretorUtils.scriptError(formattedScriptCommand,"Object name must be provided to 'end spawn object_name'");
            }
            string objectName = formattedScriptCommand.Parameters[1];
            if (!commandExecutionState.SpawnedObjects.ContainsKey(objectName)) {
                ActionScriptInterpretorUtils.scriptError(formattedScriptCommand,$"'end spawn {objectName}' was called prior to 'spawn {objectName}");
            }
            GameObject gameObject = commandExecutionState.SpawnedObjects[objectName];
            GameObject.Destroy(gameObject);
            commandExecutionState.SpawnedObjects.Remove(objectName);
        }

        public void execute(ActionScriptDescriptionParser actionScriptDescriptionParser)
        {
            if (formattedScriptCommand.Parameters.Length == 0) {
                return;
            }
            if (formattedScriptCommand.Parameters[0].Equals("if")) {
                actionScriptDescriptionParser.ParseStageDictCollections[actionScriptDescriptionParser.ParseStage].Add(new ActionDictCollection(
                    prefix: "Otherwise, ",
                    suffix: null,
                    ParseStageUtils.getTargetTypes(actionScriptDescriptionParser.ParseStage),
                    false
                ));
            } else if (formattedScriptCommand.Parameters[0].Equals("select")) {
                if (actionScriptDescriptionParser.ParseStage != ParseStage.Selection) {
                    throw new Exception("'end select' called prior to select");
                }
                actionScriptDescriptionParser.ParseStage = ParseStage.PostSelect;
            }
        }
    }
}

