using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions.Script;
using Actions;
using Levels.Combat;
using Creatures;
using Actions.Script.Execution;

namespace Actions.MCTS {
    public class GameState
    {
        private CreatureMoveOrder creatureMoveOrder;
        public int Moves;
        public int LastMove;
        public GameState(CreatureMoveOrder creatureMoveOrder)
        {
            this.creatureMoveOrder = creatureMoveOrder;
        }

        public bool isGameOver() {
            return false;
        }
        public List<GameMove> getPossibleMoves() {
            string id = creatureMoveOrder.getCurrentCombatCreature().EquipedCreeture.Creeture.getId();
            CreatureActionCollection creatureActionCollection = ActionRegistry.getInstance().getAction<CreatureActionCollection>(id);
            List<GameMove> moves = new List<GameMove>();
            foreach (ScriptedAction scriptedAction in creatureActionCollection.actions) {
                moves.AddRange(getMovesOfAction(scriptedAction));
            }
            return moves;
        }
        public List<GameMove> getMovesOfAction(ScriptedAction scriptedAction) {
            Stack<ScriptCommand> commands = ScriptCommandFactory.parseCommands(scriptedAction.ActionScript);
            while (commands.Count > 0) {
                ScriptCommand scriptCommand = commands.Pop();
                if (scriptCommand is not SelectCommand selectCommand) {
                    continue;
                }
                (int targets, CreatureSelectionType targetType, bool random, bool targetSelf) = SelectCommand.parse(selectCommand.getFormattedScriptCommand());
                List<CreatureInCombat> selectableCreatures = creatureMoveOrder.getSelectableCreatures(targetType,random,targetSelf);
                // This assumes choosing max targets is always optimal (which is most likely is)
                List<List<CreatureInCombat>> choicePermutations = GameStateUtils.GeneratePermutations<CreatureInCombat>(selectableCreatures,targets);
                
                List<GameMove> moves = new List<GameMove>();
                foreach (List<CreatureInCombat> permutation in choicePermutations) {
                    //moves.Add(new GameMove());
                } 
                return moves;
            }
            Debug.LogWarning($"{scriptedAction.name} did not have a select command");
            return new List<GameMove>();
        }
        public GameState applyMove(GameMove gameMove) {
            /*
            CreatureMoveOrder simulationMoveOrder = this.creatureMoveOrder.deepCopy();
            SimulatedExecutionState simulatedExecutionState = new SimulatedExecutionState(
                gameMove.ScriptedAction,
                creatureMoveOrder,
                true,
                gameMove.Selector
            );
            return new GameState(simulationMoveOrder);
            */
            return null;
        }
        public int getWinner() {
            return 0;
        }
    }

    public class GameMove {
        private ScriptedAction scriptedAction;
        private ISimulatedSelector selector;
        public GameMove(ScriptedAction scriptedAction, ISimulatedSelector selector)
        {
            this.scriptedAction = scriptedAction;
            this.selector = selector;
        }

        public ScriptedAction ScriptedAction { get => scriptedAction; }
        public ISimulatedSelector Selector { get => selector;}
    }

    
    public static class GameStateUtils {
        public static List<List<T>> GeneratePermutations<T>(List<T> elements, int k)
        {
            List<List<T>> result = new List<List<T>>();
            PermuteHelper<T>(new List<T>(), elements, k, result);
            return result;
        }

        private static void PermuteHelper<T>(List<T> current, List<T> elements, int k, List<List<T>> result)
        {
            if (current.Count == k)
            {
                result.Add(new List<T>(current));
                return;
            }
            for (int i = 0; i < elements.Count; i++)
            {
                List<T> newElements = new List<T>(elements);
                List<T> newCurrent = new List<T>(current) { elements[i] };
                newElements.RemoveAt(i);
                PermuteHelper(newCurrent, newElements, k, result);
            }
        }
    }
}
