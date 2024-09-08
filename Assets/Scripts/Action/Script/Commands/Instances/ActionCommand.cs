using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions.Objects;
using Actions.Script.Execution;

namespace Actions.Script {
    public class ActionCommand : DelayScriptCommand
    {
        public ActionCommand(FormattedScriptCommand formattedScriptCommand) : base(formattedScriptCommand)
        {
        }
        public static (string objectName, string actionName) parse(FormattedScriptCommand formattedScriptCommand) {
            List<object> parsedParameters = ActionScriptParseUtils.parseOrdered(
                new List<ParseInstruction>{
                    new ParseInstruction(ParseType.String,"Object",true),
                    new ParseInstruction(ParseType.String,"Action",true),
                },
                formattedScriptCommand.Parameters,
                formattedScriptCommand
            );
            string objectName = (string) parsedParameters[0];
            string actionName = (string) parsedParameters[1];
            return (objectName,actionName);
        }
        public override IEnumerator execute(LiveCommandExecutionState commandExecutionState)
        {
            (string objectName, string actionName) = parse(formattedScriptCommand);
            if (!commandExecutionState.SpawnedObjects.ContainsKey(objectName)) {
                ActionScriptInterpretorUtils.scriptError(formattedScriptCommand,$"Cannot begin action on object {objectName} as it is not spawned");
            }
            GameObject gameObject = commandExecutionState.SpawnedObjects[objectName];
            if (gameObject == null) {
                yield break;
            }
            switch (actionName) {
                case "move":
                    IActionSpawnObject actionSpawnObject = gameObject.GetComponent<IActionSpawnObject>();
                    if (actionSpawnObject == null) {
                        ActionScriptInterpretorUtils.scriptError(formattedScriptCommand,"Object has no move component");
                    }
                    if (commandExecutionState.TargetCreature == null) {
                        yield break;
                    }
                    yield return actionSpawnObject.moveToTarget(commandExecutionState.TargetCreature.transform);
                    break;
                default:
                    ActionScriptInterpretorUtils.scriptError(formattedScriptCommand,$"{actionName} is not a valid action in this context");
                    break;
            }
        }
    }
}

