using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Levels.Combat;
using System;
using System.Linq;
using Creatures;
using Items;


using Actions.Script.Execution;

namespace Actions.Script {
    public static class ActionScriptInterpretorUtils
    {
        public static void scriptError(FormattedScriptCommand scriptCommand, string description) {
            throw new System.Exception($"Script execution error at line {scriptCommand.Line+1} '{description}'");
        }        
    }
    public class CommandExecutionState {
        private CombatLevelController combatLevelController;
        public Stack<ScriptCommand> CommandStack;
        private Dictionary<string, GameObject> objectPrefabs;
        private Dictionary<string, RuntimeAnimatorController> animations;
        private Dictionary<string, AudioClip> sounds;
        private Dictionary<string,GameObject> spawnedObjects = new Dictionary<string, GameObject>();
        public CreatureSelector CreatureSelector;
        public bool PausedForSelection;
        public bool Run = true;
        private RuntimeAnimatorController defaultAnimation;
        private CreatureCombatObject selfCreature;
        public SelectionCommandLoop SubStack;
        public bool Complete {get => CommandStack.Count==0 && SubStack.iterations==0;}
        public CreatureCombatObject SelfCreature { get => selfCreature;}
        public CreatureCombatObject TargetCreature {get => CreatureSelector.Creatures[SubStack.iterations-1];}
        public Dictionary<string, GameObject> ObjectPrefabs { get => objectPrefabs; }
        public Dictionary<string, GameObject> SpawnedObjects { get => spawnedObjects; }
        public RuntimeAnimatorController DefaultAnimation { get => defaultAnimation; }
        public Dictionary<string, RuntimeAnimatorController> Animations { get => animations; }
        public Dictionary<string, AudioClip> Sounds { get => sounds; }
        public SpecialActionExecutor SpecialActionExecutor { get => specialActionExecutor; }
        public CombatLevelController CombatLevelController { get => combatLevelController; }
        private SpecialActionExecutor specialActionExecutor;

        public CommandExecutionState(ScriptedAction scriptedAction, CreatureCombatObject selfCreature, CombatLevelController combatLevelController, bool executeSpecialActions)
        {
            this.CommandStack = ScriptCommandFactory.parseCommands(scriptedAction.ActionScript);
            this.objectPrefabs = scriptedAction.PrefabDict;
            this.animations = scriptedAction.AnimationDict;
            this.sounds = scriptedAction.SoundDict;
            this.selfCreature = selfCreature;
            defaultAnimation = selfCreature.Animator.runtimeAnimatorController;
            this.combatLevelController = combatLevelController;
            if (executeSpecialActions) {
                specialActionExecutor = new SpecialActionExecutor();
            }
        }
        public IEnumerator executeSection() {
            if (SubStack != null) {
                SubStack.iterations = CreatureSelector.Creatures.Count;
                List<ScriptCommand> cachedCommands = new List<ScriptCommand>();
                while (SubStack.commands.Count > 0) {
                    cachedCommands.Insert(0,SubStack.commands.Pop());
                }
                while (SubStack.iterations > 0) {
                    foreach (ScriptCommand scriptCommand in cachedCommands) {
                        SubStack.commands.Push(scriptCommand);
                    }
                    while (SubStack.commands.Count > 0) {
                        if (selfCreature == null) {
                            yield break;
                        }
                        ScriptCommand command = SubStack.commands.Pop();
                        yield return ScriptCommandUtils.executeCommand(this, command);
                    }
                    SubStack.iterations--;
                    if (specialActionExecutor != null) {
                        yield return specialActionExecutor.execute();
                        specialActionExecutor = new SpecialActionExecutor();
                    }
                }
            }
            PausedForSelection = false;
            while (CommandStack.Count > 0 && !PausedForSelection) {
                ScriptCommand command = CommandStack.Pop();
                yield return ScriptCommandUtils.executeCommand(this, command);
                if (!Run) {
                    break;
                }
            }
            foreach (GameObject spawnedObject in spawnedObjects.Values) {
                GameObject.Destroy(spawnedObject);
            }
        }
        public CreatureSelector getCurrentSelector() {
            return CreatureSelector;
        }
        public CreatureCombatObject getActionTarget(ActionTargetType actionTargetType) {
            switch (actionTargetType) {
                case ActionTargetType.Self:
                    return selfCreature;
                case ActionTargetType.Target:
                    return TargetCreature;
                default:
                    throw new Exception($"{actionTargetType} was not covered by switch statement");
            }
        }
        public CreatureCombatObject getActionTarget(bool self) {
            if (self) {
                return selfCreature;
            } else {
                return TargetCreature;
            }
        }
        public CreatureCombatObject getCreatureFromIndicator(FormattedScriptCommand scriptCommand, string objIndicator) {
            switch (objIndicator) {
                case "self":
                    return selfCreature;
                case "target":
                    return TargetCreature;
                default:
                    ActionScriptInterpretorUtils.scriptError(scriptCommand,$"{objIndicator} is not a valid object indiciator");
                    return null;
            }
        }
    }
    
}

