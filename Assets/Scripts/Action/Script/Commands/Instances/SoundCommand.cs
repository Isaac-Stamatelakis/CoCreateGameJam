using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions.Script {
    public class SoundCommand : InstantScriptCommand
    {
        public SoundCommand(FormattedScriptCommand formattedScriptCommand) : base(formattedScriptCommand)
        {
        }

        public override void execute(CommandExecutionState commandExecutionState)
        {
            string soundName = parse(formattedScriptCommand);
            if (!commandExecutionState.Sounds.ContainsKey(soundName)) {
                ActionScriptInterpretorUtils.scriptError(formattedScriptCommand,$"Could not find sound '{soundName}'");
            }
            AudioClip audioClip = commandExecutionState.Sounds[soundName];
            commandExecutionState.SelfCreature.AudioSource.clip = audioClip;
            commandExecutionState.SelfCreature.AudioSource.Play();
        }
        public static string parse(FormattedScriptCommand formattedScriptCommand) {
            List<object> parsedParameters = ActionScriptParseUtils.parseOrdered(
                new List<ParseInstruction>{
                    new ParseInstruction(ParseType.String,"Name",true),
                },
                formattedScriptCommand.Parameters,
                formattedScriptCommand
            );
            string soundName = (string) parsedParameters[0];
            return soundName;
        }
    }
}

