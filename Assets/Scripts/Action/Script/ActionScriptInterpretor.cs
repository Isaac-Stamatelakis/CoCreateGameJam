using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Levels.Combat;
using Creatures.Actions;
using System;
using System.Linq;
using Creatures;
using Items;
using Actions.Objects;

namespace Actions.Script {
    public static class ActionScriptInterpretorUtils
    {
        public static void scriptError(ScriptCommand scriptCommand, string description) {
            throw new System.Exception($"Script execution error at line {scriptCommand.Line+1} '{description}'");
        }        
    }

    public class ScriptCommand {
        private int line;
        private string command;
        private string[] parameters;
        public ScriptCommand(string command, string[] parameters, int line) {
            this.command = command;
            this.parameters = parameters;
            this.line = line;
        }

        public string Command { get => command;}
        public string[] Parameters { get => parameters;}
        public int Line {get => line;}
    }
    public class CommandExecutionState {
        private Stack<ScriptCommand> commandStack;
        private Dictionary<string, GameObject> objectPrefabs;
        private Dictionary<string, RuntimeAnimatorController> animations;
        private Dictionary<string, AudioClip> sounds;
        private Dictionary<string,GameObject> spawnedObjects = new Dictionary<string, GameObject>();
        private CreatureSelector creatureSelector;
        private bool pausedForSelection;
        private RuntimeAnimatorController defaultAnimation;
        private CreatureCombatObject selfCreature;
        private SelectionCommandLoop subStack;
        public bool Complete {get => commandStack.Count==0 && subStack.iterations==0;}
        public CommandExecutionState(ScriptedAction scriptedAction, CreatureCombatObject selfCreature)
        {
            this.commandStack = ActionScriptParseUtils.parseCommands(scriptedAction.ActionScript);
            this.objectPrefabs = scriptedAction.PrefabDict;
            this.animations = scriptedAction.AnimationDict;
            this.sounds = scriptedAction.SoundDict;
            this.selfCreature = selfCreature;
            defaultAnimation = selfCreature.Animator.runtimeAnimatorController;
        }

        public IEnumerator executeSection() {
            if (subStack != null) {
                subStack.iterations = creatureSelector.Creatures.Count;
                List<ScriptCommand> cachedCommands = new List<ScriptCommand>();
                while (subStack.commands.Count > 0) {
                    cachedCommands.Insert(0,subStack.commands.Pop());
                }
                while (subStack.iterations > 0) {
                    foreach (ScriptCommand scriptCommand in cachedCommands) {
                        subStack.commands.Push(scriptCommand);
                    }
                    while (subStack.commands.Count > 0) {
                        ScriptCommand command = subStack.commands.Pop();
                        yield return executeCommand(command);
                    }
                    subStack.iterations--;
                }
            }
            pausedForSelection = false;
            while (commandStack.Count > 0 && !pausedForSelection) {
                ScriptCommand scriptCommand = commandStack.Pop();
                yield return executeCommand(scriptCommand);
            }
            if (commandStack.Count == 0) {
                if (spawnedObjects.Count > 0) {
                    string creaturesNotFreed = "";
                    foreach (string key in spawnedObjects.Keys) {
                        creaturesNotFreed += $"{key}, ";
                    }
                    throw new Exception($"{creaturesNotFreed} were not freed");
                }
            }
        }
        public CreatureSelector getCurrentSelector() {
            return creatureSelector;
        }
        private IEnumerator executeCommand(ScriptCommand scriptCommand) {
            Debug.Log(scriptCommand.Command);
            switch (scriptCommand.Command) {
                case "select":
                    executeSelect(scriptCommand);
                    break;
                case "spawn":
                    executeSpawn(scriptCommand);
                    break;
                case "if":
                    executeIf(scriptCommand);
                    break;
                case "end":
                    executeEnd(scriptCommand);
                    break;
                case "move":
                    yield return executeMove(scriptCommand);
                    break;
                case "attack":
                    executeAttack(scriptCommand);
                    break;
                case "action":
                    yield return executeAction(scriptCommand);
                    break;
                case "animation":
                    yield return executeAnimation(scriptCommand);
                    break;
                case "sound":
                    executeSound(scriptCommand);
                    break;
                case "status":
                    break;
                case "mana":
                    break;
                case "heal":
                    break;
                default:
                    ActionScriptInterpretorUtils.scriptError(scriptCommand,$"{scriptCommand.Command} is not a valid command");
                    break;
            }
        }

        private void executeSelect(ScriptCommand scriptCommand) {
            (int targets, string type, bool random, bool targetSelf) = ActionScriptCommandParser.parseSelectCommand(scriptCommand);
            CreatureSelectionType targetType = GlobalUtils.stringToEnum<CreatureSelectionType>(type);
            creatureSelector = new CreatureSelector(
                targetType: targetType,
                allowSelf: targetSelf,
                maxTargets: targets
            );
            pausedForSelection = true;
            Stack<ScriptCommand> reversedStack = new Stack<ScriptCommand>();
            while (commandStack.Count > 0) {
                ScriptCommand command = commandStack.Pop();
                reversedStack.Push(command);
                if (!command.Command.Equals("end")) {
                    continue;
                }
                if (command.Parameters.Length == 0) {
                    ActionScriptInterpretorUtils.scriptError(command,"end command has no command to end");
                }
                if (!command.Parameters[0].Equals("select")) {
                    continue;
                }
                reversedStack.Pop(); // Remove end
                break;
            }
            Stack<ScriptCommand> subCommandStack = new Stack<ScriptCommand>();
            while (reversedStack.Count > 0) {
                subCommandStack.Push(reversedStack.Pop());
            }
            subStack = new SelectionCommandLoop(subCommandStack);
        }
        
        private void executeSpawn(ScriptCommand scriptCommand) {
            List<object> parsedParameters = ActionScriptParseUtils.parseOrdered(
                new List<ParseInstruction>{
                    new ParseInstruction(ParseType.String,"Name",true)
                },
                scriptCommand.Parameters,
                scriptCommand
            );
            string spawnName = (string) parsedParameters[0];
            if (!objectPrefabs.ContainsKey(spawnName)) {
                ActionScriptInterpretorUtils.scriptError(scriptCommand,$"Prefab {spawnName} could not be found");
            }
            if (spawnedObjects.ContainsKey(spawnName)) {
                ActionScriptInterpretorUtils.scriptError(scriptCommand,$"Prefab {spawnName} is already spawned");
            }
            GameObject instantiated = GameObject.Instantiate(objectPrefabs[spawnName]);
            instantiated.transform.SetParent(selfCreature.transform,false);
            spawnedObjects[spawnName] = instantiated;
        }

        private void executeIf(ScriptCommand scriptCommand) {
            (string first, string booleanOperator, string second) = ActionScriptCommandParser.parseIfCommand(scriptCommand);
            bool statementPassed = false;
            if (booleanOperator.Equals("has")) {
                string creatureIndicator = first;
                string statusIndicator = second;
                CreatureCombatObject creatureCombatObject = getCreatureFromIndicator(scriptCommand,creatureIndicator);
                statementPassed = creatureCombatObject.CreatureInCombat.hasStatusEffect(statusIndicator);
            } else if (
                booleanOperator.Equals("<")  || 
                booleanOperator.Equals(">")  || 
                booleanOperator.Equals("<=") || 
                booleanOperator.Equals(">=") || 
                booleanOperator.Equals("==")
            ) {
                double a = parseDoubleValue(first,scriptCommand);
                double b = parseDoubleValue(second,scriptCommand);
                switch (booleanOperator) {
                    case "<":
                        statementPassed = a < b;
                        break;
                    case ">":
                        statementPassed = a > b;
                        break;
                    case "<=":
                        statementPassed = a <= b;
                        break;
                    case ">=":
                        statementPassed = a >= b;
                        break;
                    case "==":
                        statementPassed = a == b;
                        break;
                }
            } else if (booleanOperator.Equals("is")) {
                string creatureIndicator = first;
                string statusIndicator = second;
                CreatureCombatObject creatureCombatObject = getCreatureFromIndicator(scriptCommand,creatureIndicator);
            } else {
                ActionScriptInterpretorUtils.scriptError(scriptCommand,$"{booleanOperator} is not a valid boolean operator");
            }
            if (statementPassed) {
                return;
            }
            if (subStack != null) {
                skipIfStatement(scriptCommand,subStack.commands);
            } else {
                skipIfStatement(scriptCommand,commandStack);
            }
            
        }

        private void skipIfStatement(ScriptCommand ifCommand, Stack<ScriptCommand> commands) {
            while (commands.Count > 0) {
                ScriptCommand scriptCommand = commands.Pop();
                if (scriptCommand.Command != "end") {
                    continue;
                }
                if (scriptCommand.Parameters.Length == 0) {
                    ActionScriptInterpretorUtils.scriptError(ifCommand,"'end' must be called with atleast one parameter");
                }
                if (scriptCommand.Parameters[0].Equals("if")) {
                    return;
                }
            }
            // Only gets here if commands are emptied ie no 'end if' statement was present in script
            ActionScriptInterpretorUtils.scriptError(ifCommand,"'if' was called with no 'end if'");
        }

        private double parseDoubleValue(string val, ScriptCommand scriptCommand) {
            try {
                return Convert.ToDouble(val);
            } catch (FormatException) {

            }
            try {
                string[] split = val.Split(".");
                string objIndicator = split[0];
                string attributeIndicator = split[1];
                CreatureCombatObject creatureCombatObject = getCreatureFromIndicator(scriptCommand,objIndicator);
                return ActionScriptParseUtils.parseCreatureAttribute(scriptCommand,attributeIndicator,creatureCombatObject.CreatureInCombat);
            } catch (IndexOutOfRangeException) {
                ActionScriptInterpretorUtils.scriptError(scriptCommand,$"{val} must be of form obj.val");
            }
            return default(double);
        }

        private CreatureCombatObject getCreatureFromIndicator(ScriptCommand scriptCommand, string objIndicator) {
                switch (objIndicator) {
                    case "self":
                        return selfCreature;
                    case "target":
                        return getTarget();
                    default:
                        ActionScriptInterpretorUtils.scriptError(scriptCommand,$"{objIndicator} is not a valid object indiciator");
                        return null;
                }
        }
        private void executeAttack(ScriptCommand scriptCommand) {
            (float damage, float range, DamageType damageType, float falloff, float lifesteal) = ActionScriptCommandParser.parseAttackCommand(scriptCommand);
            CreatureCombatObject target = getTarget();
            float realDamage = UnityEngine.Random.Range(damage-range,damage+range);
            realDamage += falloff * subStack.iterations;
            target.CreatureInCombat.hit(realDamage,damageType);
            selfCreature.CreatureInCombat.heal(realDamage*lifesteal);
        }

        private IEnumerator executeMove(ScriptCommand scriptCommand) {
            if (scriptCommand.Parameters.Length == 0) {
                ActionScriptInterpretorUtils.scriptError(scriptCommand,"No object to move given");
            }
            if (scriptCommand.Parameters.Length == 1) {
                ActionScriptInterpretorUtils.scriptError(scriptCommand,"No position to move given");
            }
            string toMoveString = scriptCommand.Parameters[0];
            Transform toMoveTransform = null;
            if (toMoveString.Equals("self")) {
                toMoveTransform = selfCreature.transform;
            } else {
                if (!spawnedObjects.ContainsKey(toMoveString)) {
                    ActionScriptInterpretorUtils.scriptError(scriptCommand,$"Could not find object {toMoveString}");
                }
                toMoveTransform = spawnedObjects[toMoveString].transform;
            }
            string toGoString = scriptCommand.Parameters[1];
            Vector3 toGoPosition = Vector3.zero;
            if (toGoString.Equals("self")) {
                toGoPosition = selfCreature.transform.position;
            } else if (toGoString.Equals("target")) {
                toGoPosition = getTarget().transform.position;
            } else {
                try {
                    List<float> exactPosition = ActionScriptParseUtils.parseArray(toGoString);
                    if (exactPosition.Count != 2) {
                        ActionScriptInterpretorUtils.scriptError(scriptCommand,"Exact position to go to must have 2 values");
                    }
                    toGoPosition = new Vector3(exactPosition[0],exactPosition[1],0);
                } catch (FormatException) {
                    ActionScriptInterpretorUtils.scriptError(scriptCommand,"To go position was not 'self','target' or '[x,y]'");
                }
            }
            string[] additionalParameters = ActionScriptParseUtils.reduceArraySize<string>(scriptCommand.Parameters,2);
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
                additionalParameters,
                scriptCommand
            );
            float vel = 1;
            if (parsedParameters.ContainsKey("vel")) {
                vel = (int) parsedParameters["vel"];
            }
            if (parsedParameters.ContainsKey("off")) {
                List<float> offsetList = (List<float>) parsedParameters["off"];
                if (offsetList.Count != 2) {
                    ActionScriptInterpretorUtils.scriptError(scriptCommand,"Offset array must have exactly 2 values");
                }
                toGoPosition = toGoPosition + new Vector3(offsetList[0],offsetList[1],0);
            }
            Vector3 toMovePosition = toMoveTransform.position;
            (Vector3,int) tuple = GlobalUtils.speedAndIterationsToMove(toGoPosition,toMovePosition,vel);
            Vector3 speed = tuple.Item1;
            int iterations = tuple.Item2;
            if (toMoveString.Equals("self")) {
                while (iterations > 0) {
                    iterations--;
                    selfCreature.move(speed);
                    yield return new WaitForFixedUpdate();
                }
            } else {
                while (iterations > 0) {
                    iterations--;
                    toMoveTransform.position += speed;
                    yield return new WaitForFixedUpdate();
                }
            }
            
        }
        private IEnumerator executeAnimation(ScriptCommand scriptCommand) {
            List<object> parsedParameters = ActionScriptParseUtils.parseOrdered(
                new List<ParseInstruction>{
                    new ParseInstruction(ParseType.String,"Animation",true),
                },
                scriptCommand.Parameters,
                scriptCommand
            );
            Dictionary<string, object> optionalParameters = ActionScriptParseUtils.parseDict(
                new List<ParseInstruction>{
                    new ParseInstruction(ParseType.Boolean,"loop",false)
                },
                scriptCommand.Parameters,
                scriptCommand
            );
            RuntimeAnimatorController newAnimation = null;
            string animationName = (string) parsedParameters[0];
            if (animationName.Equals("reset")) { // Special case to reset to idle
                newAnimation = defaultAnimation;
            } else {
                if (!animations.ContainsKey(animationName)) {
                    ActionScriptInterpretorUtils.scriptError(scriptCommand,$"Could not find animation '{animationName}'");
                }
                newAnimation = animations[animationName];
            }
            bool loop = false;
            if (optionalParameters.ContainsKey("loop")) {
                loop = (bool) optionalParameters["loop"];  
            } 
            if (!loop) {
                RuntimeAnimatorController currentAnimation = selfCreature.Animator.runtimeAnimatorController;
                selfCreature.Animator.runtimeAnimatorController = newAnimation;
                while (selfCreature.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) {
                    yield return new WaitForFixedUpdate();
                }
                selfCreature.Animator.runtimeAnimatorController = currentAnimation;
            } else {
                selfCreature.Animator.runtimeAnimatorController = newAnimation;
            }
        }

        private CreatureCombatObject getTarget() {
            return creatureSelector.Creatures[subStack.iterations-1];
        }

        private IEnumerator executeAction(ScriptCommand scriptCommand) {
            List<object> parsedParameters = ActionScriptParseUtils.parseOrdered(
                new List<ParseInstruction>{
                    new ParseInstruction(ParseType.String,"Object",true),
                    new ParseInstruction(ParseType.String,"Action",true),
                },
                scriptCommand.Parameters,
                scriptCommand
            );
            string objectName = (string) parsedParameters[0];
            if (!spawnedObjects.ContainsKey(objectName)) {
                ActionScriptInterpretorUtils.scriptError(scriptCommand,$"Cannot begin action on object {objectName} as it is not spawned");
            }
            GameObject gameObject = spawnedObjects[objectName];
            string actionName = (string) parsedParameters[1];
            switch (actionName) {
                case "move":
                    IActionSpawnObject actionSpawnObject = gameObject.GetComponent<IActionSpawnObject>();
                    if (actionSpawnObject == null) {
                        ActionScriptInterpretorUtils.scriptError(scriptCommand,"Object has no move component");
                    }
                    yield return actionSpawnObject.moveToTarget(getTarget().transform);
                    break;
                default:
                    ActionScriptInterpretorUtils.scriptError(scriptCommand,$"{actionName} is not a valid action in this context");
                    break;
            }
        }

        private void executeSound(ScriptCommand scriptCommand) {
            List<object> parsedParameters = ActionScriptParseUtils.parseOrdered(
                new List<ParseInstruction>{
                    new ParseInstruction(ParseType.String,"Name",true),
                },
                scriptCommand.Parameters,
                scriptCommand
            );
            string soundName = (string) parsedParameters[0];
            if (!sounds.ContainsKey(soundName)) {
                ActionScriptInterpretorUtils.scriptError(scriptCommand,$"Could not find sound '{soundName}'");
            }
            AudioClip audioClip = sounds[soundName];
            selfCreature.AudioSource.clip = audioClip;
            selfCreature.AudioSource.Play();
        }
        
        private void executeEnd(ScriptCommand scriptCommand) {
            if (scriptCommand.Parameters.Length == 0) {
                ActionScriptInterpretorUtils.scriptError(scriptCommand,"no sub command provided to end");
            }
            string subCommand = scriptCommand.Parameters[0];
            switch (subCommand) {
                case "select":
                    break;
                case "spawn":
                    endSpawn(scriptCommand);
                    break;
                case "if":
                    // 'end if' has no execution behavior. It is skipped past if an if statement fails.
                    break;
                default:
                    ActionScriptInterpretorUtils.scriptError(scriptCommand,$"{subCommand} is not a valid end command");
                    break;
            }
        }
        private void endSpawn(ScriptCommand scriptCommand) {
            if (scriptCommand.Parameters.Length < 2) {
                ActionScriptInterpretorUtils.scriptError(scriptCommand,"Object name must be provided to 'end spawn object_name'");
            }
            string objectName = scriptCommand.Parameters[1];
            if (!spawnedObjects.ContainsKey(objectName)) {
                ActionScriptInterpretorUtils.scriptError(scriptCommand,$"'end spawn {objectName}' was called prior to 'spawn {objectName}");
            }
            GameObject gameObject = spawnedObjects[objectName];
            GameObject.Destroy(gameObject);
            spawnedObjects.Remove(objectName);
        }
        private class SelectionCommandLoop {
            public int iterations;
            public Stack<ScriptCommand> commands;

            public SelectionCommandLoop(Stack<ScriptCommand> commands)
            {
                iterations = -1;
                this.commands = commands;
            }
        }
    }
}

