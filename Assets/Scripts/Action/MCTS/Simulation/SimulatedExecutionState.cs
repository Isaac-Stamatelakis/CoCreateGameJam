using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Actions.MCTS;
using Levels.Combat;
using Actions.Script;

namespace Actions.Script.Execution {
    public class SimulatedExecutionState : CommandExecutionState<CreatureInCombat>
    {
        public float ChanceModifer;
        public SimulatedExecutionState(
            ScriptedAction scriptedAction, 
            CreatureInCombat selfCreature, 
            bool executeEquipmentActions, 
            CreatureSelector<CreatureInCombat> selector) : base(scriptedAction, selfCreature, executeEquipmentActions)
        {
            CreatureSelector = selector;
        }

        public float getActionEffectiveness() {
            float val = ChanceModifer;
            if (CreatureSelector is SimulatedRandomSelector<CreatureInCombat> randomSelector) {
                val *= randomSelector.getActionModification();
            }
            return val;
        }
        
        public void execute() {
            /*
            while (CommandStack.Count > 0) {
                ScriptCommand scriptCommand = CommandStack.Pop();
                if (scriptCommand is SelectCommand selectCommand) {
                    selectCommand.execute(this);
                    break;
                }
                if (scriptCommand is not ISimultableCommand simultableCommand) {
                    continue;
                }
                simultableCommand.execute(this);
            }
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
                        ScriptCommand command = SubStack.commands.Pop();
                        executeCommand(command);
                    }
                    SubStack.iterations--;
                    if (equipmentActionExecutor != null) {
                        yield return equipmentActionExecutor.execute();
                        equipmentActionExecutor = new EquipmentActionExecutor();
                    }
                }
            }
            while (CommandStack.Count > 0 && !PausedForSelection) {
                ScriptCommand command = CommandStack.Pop();
                yield return ScriptCommandUtils.executeCommand(this, command);
                if (!Run) {
                    break;
                }
            }
            */
        }
        

        protected void executeCommand(ScriptCommand scriptCommand) {
            if (scriptCommand is not ISimultableCommand simultableCommand) {
                return;
            }
            simultableCommand.execute(this);
        }


        
    }
}

