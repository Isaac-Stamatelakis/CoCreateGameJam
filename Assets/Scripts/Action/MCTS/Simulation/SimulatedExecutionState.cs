using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Actions.MCTS;
using Levels.Combat;
using Actions.Script;

namespace Actions.Script.Execution {
    public class SimulatedExecutionState : ICommandExecuteState
    {
        private Stack<ScriptCommand> commandStack;
        private Stack<ScriptCommand> loopMemory;
        private CreatureMoveOrder creatureMoveOrder;
        private SpecialActionExecutor specialActionExecutor;
        private ISimulatedSelector simulatedSelector;
        private int loopIterations;
        private bool selectExecuted;
        public SimulatedExecutionState(
            ScriptedAction scriptedAction, 
            CreatureMoveOrder creatureMoveOrder,
            bool executeSpecialActions, 
            ISimulatedSelector simulatedSelector
        ) {
            this.commandStack = ScriptCommandFactory.parseCommands(scriptedAction.ActionScript);
            this.creatureMoveOrder = creatureMoveOrder;
            this.simulatedSelector = simulatedSelector;
            if (executeSpecialActions) {
                specialActionExecutor = new SpecialActionExecutor();
            }
        }

        public void execute() {
            while (commandStack.Count > 0) {
                ScriptCommand scriptCommand = commandStack.Pop();
                bool inSelectionLoop = loopIterations >= 0 && loopIterations < simulatedSelector.maxIterations();
                if (scriptCommand is SelectCommand selectCommand) {
                    if (selectExecuted) {
                        throw new System.Exception("Only one select statement per script is allowed");
                    }
                    selectExecuted = true;
                    loopIterations = 0;
                    continue;
                }
                if (inSelectionLoop) {
                    loopMemory.Push(scriptCommand);
                }
                if (scriptCommand is EndCommand endCommand && endCommand.endsStatement("select")) {
                    loopIterations ++;
                    Stack<ScriptCommand> reversedMemory = new Stack<ScriptCommand>();
                    while (loopMemory.Count > 0) {
                        reversedMemory.Push(loopMemory.Pop());
                    }
                    while (reversedMemory.Count > 0) {
                        commandStack.Push(reversedMemory.Pop());
                    }
                }
                if (scriptCommand is not ISimultableCommand simultableCommand) {
                    continue;
                }
                simultableCommand.execute(this);
            }
        }

        public CreatureInCombat getSelfCreature()
        {
            return creatureMoveOrder.getCurrentCombatCreature();
        }

        public CreatureInCombat getTargetCreature()
        {
            if (loopIterations < 0 || loopIterations > simulatedSelector.maxIterations()) {
                return null;
            }
            return simulatedSelector.getTarget(loopIterations);
        }
    }
}

