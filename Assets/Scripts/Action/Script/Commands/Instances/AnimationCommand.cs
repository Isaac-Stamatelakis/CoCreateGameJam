using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions.Script {
    public class AnimationCommand : DelayScriptCommand
    {
        public AnimationCommand(FormattedScriptCommand formattedScriptCommand) : base(formattedScriptCommand)
        {
        }
        private RuntimeAnimatorController getAnimation(string animationName, LiveCommandExecutionState commandExecutionState) {
            if (animationName.Equals("reset")) { // Special case to reset to idle
                return commandExecutionState.DefaultAnimation;
            } else {
                if (!commandExecutionState.Animations.ContainsKey(animationName)) {
                    ActionScriptInterpretorUtils.scriptError(formattedScriptCommand,$"Could not find animation '{animationName}'");
                }
                return commandExecutionState.Animations[animationName];
            }
        }
        public override IEnumerator execute(LiveCommandExecutionState commandExecutionState)
        {
            (string animationName, bool loop) = parse(formattedScriptCommand);
            RuntimeAnimatorController animatorController = getAnimation(animationName,commandExecutionState);
            Animator animator = commandExecutionState.SelfCreature.Animator;
            if (!loop) {              
                RuntimeAnimatorController currentAnimation = animator.runtimeAnimatorController;
                animator.runtimeAnimatorController = animatorController;
                while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) {
                    yield return new WaitForFixedUpdate();
                }
                animator.runtimeAnimatorController = currentAnimation;
            } else {
                animator.runtimeAnimatorController = animatorController;
            }
        }

        public static (string animation,bool loop) parse(FormattedScriptCommand formattedScriptCommand) {
            List<object> parsedParameters = ActionScriptParseUtils.parseOrdered(
                new List<ParseInstruction>{
                    new ParseInstruction(ParseType.String,"Animation",true),
                },
                formattedScriptCommand.Parameters,
                formattedScriptCommand
            );
            string animation = (string) parsedParameters[0];
            Dictionary<string, object> optionalParameters = ActionScriptParseUtils.parseDict(
                new List<ParseInstruction>{
                    new ParseInstruction(ParseType.Boolean,"loop",false)
                },
                formattedScriptCommand.Parameters,
                formattedScriptCommand
            );
            bool loop = false;
            if (optionalParameters.ContainsKey("loop")) {
                loop = (bool) optionalParameters["loop"];
            }
            return (animation,loop);
        }
    }

}
