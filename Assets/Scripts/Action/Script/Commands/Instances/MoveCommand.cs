using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Actions.Script {


    public class MoveCommand : DelayScriptCommand
    {
        public MoveCommand(FormattedScriptCommand formattedScriptCommand) : base(formattedScriptCommand)
        {
        }

        public static (string nameToMove, string nameToGo, float velocity, Vector3 offset) parse(FormattedScriptCommand formattedScriptCommand) {
            List<object> ordered = ActionScriptParseUtils.parseOrdered(
                new List<ParseInstruction>{
                    new ParseInstruction(ParseType.String,"ToMove",true),
                    new ParseInstruction(ParseType.String,"ToGo",true),
                },
                formattedScriptCommand.Parameters,
                formattedScriptCommand
            );
            string nameToMove = (string) ordered[0];
            string nameToGo = (string) ordered[1];
            Dictionary<string,object> parsedParameters = ActionScriptParseUtils.parseDict(
                new List<ParseInstruction>{
                    new ParseInstruction(ParseType.Integer,
                    "vel",
                    false
                    ),
                    new ParseInstruction(ParseType.IntegerArray,
                    "off",
                    false
                    )
                },
                formattedScriptCommand.Parameters,
                formattedScriptCommand
            );
            float vel = 1;
            if (parsedParameters.ContainsKey("vel")) {
                vel = (float) parsedParameters["vel"];
            }
            Vector3 offset = Vector3.zero;
            if (parsedParameters.ContainsKey("off")) {
                List<int> integers = (List<int>) parsedParameters["off"];
                offset.x = integers[0];
                offset.y = integers[1];
            }
            return (nameToMove,nameToGo,vel,offset);
        }
        private Transform getMoveTransform(string nameToMove, CommandExecutionState commandExecutionState) {
            if (nameToMove.Equals("self")) {
                return commandExecutionState.SelfCreature.transform;
            } else {
                if (!commandExecutionState.SpawnedObjects.ContainsKey(nameToMove)) {
                    ActionScriptInterpretorUtils.scriptError(formattedScriptCommand,$"Could not find object {nameToMove}");
                }
                return commandExecutionState.SpawnedObjects[nameToMove].transform;
            }
        }
        private Vector3 getMovePosition(string nameToGo, CommandExecutionState commandExecutionState) {
            if (nameToGo.Equals("self")) {
                return commandExecutionState.SelfCreature.transform.position;
            } else if (nameToGo.Equals("target")) {
                return commandExecutionState.TargetCreature.transform.position;
            } else {
                try {
                    List<float> exactPosition = ActionScriptParseUtils.parseArray(nameToGo);
                    if (exactPosition.Count != 2) {
                        ActionScriptInterpretorUtils.scriptError(formattedScriptCommand,"Exact position to go to must have 2 values");
                    }
                    return new Vector3(exactPosition[0],exactPosition[1],0);
                } catch (FormatException) {
                    ActionScriptInterpretorUtils.scriptError(formattedScriptCommand,"To go position was not 'self','target' or '[x,y]'");
                }
            }
            return Vector3.zero;
        }
        public override IEnumerator execute(CommandExecutionState commandExecutionState)
        {
            
            (string nameToMove, string nameToGo, float velocity, Vector3 offset) = parse(formattedScriptCommand);
            Transform toMove = getMoveTransform(nameToMove,commandExecutionState);
            Vector3 toMovePosition = getMovePosition(nameToGo,commandExecutionState) + offset;
            (Vector3,int) tuple = GlobalUtils.speedAndIterationsToMove(toMovePosition,toMove.transform.position,velocity);
            Vector3 speed = tuple.Item1;
            int iterations = tuple.Item2;
            if (nameToMove.Equals("self")) {
                while (iterations > 0) {
                    iterations--;
                    commandExecutionState.SelfCreature.move(speed);
                    yield return new WaitForFixedUpdate();
                }
            } else {
                while (iterations > 0) {
                    iterations--;
                    toMove.position += speed;
                    yield return new WaitForFixedUpdate();
                }
            }
        }
    }
}