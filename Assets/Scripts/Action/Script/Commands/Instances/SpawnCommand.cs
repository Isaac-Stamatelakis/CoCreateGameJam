using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Levels.Combat;

namespace Actions.Script {
    public class SpawnCommand : InstantScriptCommand
    {
        public SpawnCommand(FormattedScriptCommand formattedScriptCommand) : base(formattedScriptCommand)
        {
            
        }

        public override void execute(LiveCommandExecutionState commandExecutionState)
        {
            string spawnName = parse(formattedScriptCommand);
            if (!commandExecutionState.ObjectPrefabs.ContainsKey(spawnName)) {
                ActionScriptInterpretorUtils.scriptError(formattedScriptCommand,$"Prefab {spawnName} could not be found");
            }
            if (!commandExecutionState.ObjectPrefabs.ContainsKey(spawnName)) {
                ActionScriptInterpretorUtils.scriptError(formattedScriptCommand,$"Prefab {spawnName} is already spawned");
            }
            if (commandExecutionState.SelfCreature == null) {
                // Prevents debuging errors from calling actions on objects which are not spawned
                commandExecutionState.SpawnedObjects[spawnName] = null;
                return;
            }
            GameObject instantiated = GameObject.Instantiate(commandExecutionState.ObjectPrefabs[spawnName]);
            instantiated.transform.SetParent(CombatLevelController.Instance.SpawnedObjectContainer,false);
            instantiated.transform.position += commandExecutionState.SelfCreature.transform.position;
            commandExecutionState.SpawnedObjects[spawnName] = instantiated;
        }

        public static string parse(FormattedScriptCommand formattedScriptCommand) {
            List<object> parsedParameters = ActionScriptParseUtils.parseOrdered(
                new List<ParseInstruction>{
                    new ParseInstruction(ParseType.String,"Name",true)
                },
                formattedScriptCommand.Parameters,
                formattedScriptCommand
            );
            string spawnName = (string) parsedParameters[0];
            return spawnName;
        }

    }

}
