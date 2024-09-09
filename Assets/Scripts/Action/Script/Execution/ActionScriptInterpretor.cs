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
        public CommandExecutionState(Stack<ScriptCommand> commandStack, T selfCreature, bool executeEquipmentActions) {
            this.CommandStack = commandStack;
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
            return (ICombatCreature)CreatureSelector.getTarget(SubStack.Iterations-1);
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
        public CreatureCombatObject TargetCreature => (CreatureCombatObject) CreatureSelector.getTarget(SubStack.Iterations-1);
        public CreatureCombatObject SelfCreature => selfCreature;
        public float? SelectionPeriod;
        public LiveCommandExecutionState(
            ScriptedAction scriptedAction, 
            CreatureCombatObject selfCreature,
            bool executeEquipmetnActions
        ) : base (ScriptCommandFactory.parseCommands(scriptedAction.ActionScript),selfCreature,executeEquipmetnActions)
        {
            this.objectPrefabs = scriptedAction.PrefabDict;
            this.animations = scriptedAction.AnimationDict;
            this.sounds = scriptedAction.SoundDict;
            defaultAnimation = selfCreature.Animator.runtimeAnimatorController;
        }

        public LiveCommandExecutionState(
            CreatureCombatObject target,
            Stack<ScriptCommand> commandStack,
            CreatureCombatObject selfCreature, 
            bool executeEquipmentActions,
            Dictionary<string,GameObject> objectPrefabs,
            Dictionary<string,RuntimeAnimatorController> animations,
            Dictionary<string,AudioClip> sounds,
            RuntimeAnimatorController defaultAnimation
        ) : base(commandStack,selfCreature,executeEquipmentActions) {
            this.objectPrefabs = objectPrefabs;
            this.animations = animations;
            this.sounds = sounds;
            this.defaultAnimation = defaultAnimation;
            this.CreatureSelector = new CreatureSelector<CreatureCombatObject>(new List<CreatureCombatObject>{target});
            SubStack = new SelectionCommandLoop(commandStack,1);
        }

        

        private void endExecution() {
            foreach (GameObject spawnedObject in spawnedObjects.Values) {
                GameObject.Destroy(spawnedObject);
            }
        }
        public IEnumerator executeSection() {
            
            if (SubStack != null) {
                SubStack.Iterations = CreatureSelector.getCreatures().Count;
                List<ScriptCommand> cachedCommands = new List<ScriptCommand>();
                while (SubStack.commands.Count > 0) {
                    cachedCommands.Insert(0,SubStack.commands.Pop());
                }
                bool periodicSubStackExecution = SelectionPeriod != null;
                while (SubStack.Iterations > 0) {
                    Stack<ScriptCommand> subCommands = new Stack<ScriptCommand>();
                    foreach (ScriptCommand scriptCommand in cachedCommands) {
                        subCommands.Push(scriptCommand);
                    }
                    if (periodicSubStackExecution) {
                        LiveCommandExecutionState liveCommandExecutionState = new LiveCommandExecutionState(
                            (CreatureCombatObject) getTargetCreature(),
                            subCommands,
                            selfCreature,
                            equipmentActionExecutor!=null,
                            objectPrefabs,
                            animations,
                            sounds,
                            defaultAnimation
                        );
                        CombatLevelController.Instance.executeSubStackCommands(liveCommandExecutionState);
                        if (selfCreature == null || selfCreature.getHealth() <= 0) {
                            endExecution();
                            yield break;
                        }
                        yield return new WaitForSeconds((float)SelectionPeriod);
                    } else {
                        SubStack.commands = subCommands;
                        yield return executeSubSection(SubStack.commands);
                    }
                    SubStack.deIterate();
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

        public IEnumerator executeSubSection(Stack<ScriptCommand> commandStack) {
            while (commandStack.Count > 0) {
                if (selfCreature == null || selfCreature.getHealth() <= 0) {
                    endExecution();
                    yield break;
                }
                ScriptCommand command = commandStack.Pop();
                yield return ScriptCommandUtils.executeCommand(this, command);
            }
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

