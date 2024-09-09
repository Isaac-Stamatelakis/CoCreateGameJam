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
            Stack<ScriptCommand> commandStack, 
            CreatureInCombat selfCreature, 
            bool executeEquipmentActions, 
            CreatureSelector<CreatureInCombat> selector) : base(commandStack, selfCreature, executeEquipmentActions)
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
            List<ScriptCommand> subStackCommands = null;
            while (CommandStack.Count > 0) {
                ScriptCommand scriptCommand = CommandStack.Pop();
                if (scriptCommand is SelectCommand selectCommand) {
                    subStackCommands = new List<ScriptCommand>();
                    continue;
                }
                
                if (scriptCommand is EndCommand endCommand && endCommand.endsStatement("select")) {

                }
                
                if (scriptCommand is not ISimultableCommand simultableCommand) {
                    continue;
                }
                if (subStackCommands != null) {
                    subStackCommands.Add(scriptCommand);
                } else {
                    simultableCommand.execute(this);
                }
            }
            SubStack = new SelectionCommandLoop(new Stack<ScriptCommand>(),CreatureSelector.maxIterations());
            while (SubStack.Iterations > 0) {
                for (int i = subStackCommands.Count-1; i > 0; i--) {
                    SubStack.commands.Push(subStackCommands[i]);
                }
                while (SubStack.commands.Count > 0) {
                    ScriptCommand command = SubStack.commands.Pop();
                    executeCommand(command);
                }
                SubStack.deIterate();
                /*
                if (equipmentActionExecutor != null) {
                    yield return equipmentActionExecutor.execute();
                    equipmentActionExecutor = new EquipmentActionExecutor();
                }
                */
            }
            
            while (CommandStack.Count > 0) {
                ScriptCommand scriptCommand = CommandStack.Pop();
                executeCommand(scriptCommand);
            }
        }
        

        protected void executeCommand(ScriptCommand scriptCommand) {
            if (scriptCommand is not ISimultableCommand simultableCommand) {
                return;
            }
            simultableCommand.execute(this);
        }


        
    }
}

