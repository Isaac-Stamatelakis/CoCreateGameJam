using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Levels.Combat;
using System;
using System.Linq;
using Creatures;
using Items;
using Actions;


using Actions.Script.Execution;

namespace Actions.Script {
    public static class ActionScriptInterpretorUtils
    {
        public static void scriptError(FormattedScriptCommand scriptCommand, string description) {
            throw new System.Exception($"Script execution error at line {scriptCommand.Line+1} '{description}'");
        }        
    }

    public abstract class CommandExecutionState<T> : ICommandExecutionState where T : ICombatCreature {
        public Stack<ScriptCommand> CommandStack;
        public SelectionCommandLoop SubStack;
        public CreatureSelector<T> CreatureSelector;
        public bool Run = true;
        protected T selfCreature;
        public bool Complete => CommandStack.Count == 0 && (SubStack == null || SubStack.commands.Count == 0);
        protected EquipmentActionExecutor equipmentActionExecutor;
        public CommandExecutionState(ScriptedAction scriptedAction, T selfCreature, bool executeEquipmentActions) {
            this.CommandStack = ScriptCommandFactory.parseCommands(scriptedAction.ActionScript);
            this.selfCreature = selfCreature;
            if (executeEquipmentActions) {
                equipmentActionExecutor = new EquipmentActionExecutor();
            }
        }
        public ICombatCreature getSelfCreature()
        {
            return selfCreature;
        }

        public EquipmentActionExecutor getEquipmentActionExecutor()
        {
            return equipmentActionExecutor;
        }

        public Stack<ScriptCommand> getStack()
        {
            return CommandStack;
        }

        public SelectionCommandLoop getSubStack()
        {
            return SubStack;
        }

        public ICombatCreature getTargetCreature()
        {
            return (ICombatCreature)CreatureSelector.getTarget(SubStack.iterations-1);
        }

        public ICombatCreature getActionTarget(bool targetSelf) {
            if (targetSelf) {
                return getSelfCreature();
            } else {
                return getTargetCreature();
            }
        }
    }
    public class LiveCommandExecutionState : CommandExecutionState<CreatureCombatObject> {
        private Dictionary<string, GameObject> objectPrefabs;
        private Dictionary<string, RuntimeAnimatorController> animations;
        private Dictionary<string, AudioClip> sounds;
        private Dictionary<string,GameObject> spawnedObjects = new Dictionary<string, GameObject>();
        public bool PausedForSelection;
        private RuntimeAnimatorController defaultAnimation;
        public Dictionary<string, GameObject> ObjectPrefabs { get => objectPrefabs; }
        public Dictionary<string, GameObject> SpawnedObjects { get => spawnedObjects; }
        public RuntimeAnimatorController DefaultAnimation { get => defaultAnimation; }
        public Dictionary<string, RuntimeAnimatorController> Animations { get => animations; }
        public Dictionary<string, AudioClip> Sounds { get => sounds; }
        public CreatureCombatObject TargetCreature => (CreatureCombatObject) CreatureSelector.getTarget(SubStack.iterations-1);
        public CreatureCombatObject SelfCreature => selfCreature;
        public LiveCommandExecutionState(
            ScriptedAction scriptedAction, 
            CreatureCombatObject selfCreature,
            bool executeEquipmetnActions
        ) : base (scriptedAction,selfCreature,executeEquipmetnActions)
        {
            this.objectPrefabs = scriptedAction.PrefabDict;
            this.animations = scriptedAction.AnimationDict;
            this.sounds = scriptedAction.SoundDict;
            defaultAnimation = selfCreature.Animator.runtimeAnimatorController;
        }

        private void endExecution() {
            foreach (GameObject spawnedObject in spawnedObjects.Values) {
                GameObject.Destroy(spawnedObject);
            }
        }
        public IEnumerator executeSection() {
            if (SubStack != null) {
                SubStack.iterations = CreatureSelector.maxIterations();
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
                            endExecution();
                            yield break;
                        }
                        ScriptCommand command = SubStack.commands.Pop();
                        yield return ScriptCommandUtils.executeCommand(this, command);
                    }
                    SubStack.iterations--;
                    if (equipmentActionExecutor != null) {
                        yield return equipmentActionExecutor.execute();
                        equipmentActionExecutor = new EquipmentActionExecutor();
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
            endExecution();
        }

        public ManualCreatureSelector getManualCreatureSelector() {
            if (CreatureSelector is ManualCreatureSelector manualCreatureSelector) {
                return manualCreatureSelector;
            }
            Debug.LogWarning("Tried to access manual creature selector when selector was not manual");
            return null;
        }
        
    }
    
}

