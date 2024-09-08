using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions.Script;
using Creatures;
using System;
using Levels.Combat;

namespace Actions.MCTS {
    public class MonteCarloTreeSearch
    {
        private static System.Random random = new System.Random();
        private MCTSNode root;
        public GameMove getMove(GameState initalState, int maxIterations) {
            root = new MCTSNode(null,initalState);
            for (int i = 0; i < maxIterations; i++) {
                MCTSNode node = selection(root);
                if (!node.State.isGameOver()) {
                    node = expansion(node);
                    int result = simulation(node);
                    backpropagation(node,result);
                }
            }
            return root.bestChild().State.LastMove;
        }

        private MCTSNode selection(MCTSNode node) {
            while (!node.IsFullyExpanded && !node.State.isGameOver())
            {
                if (!node.IsFullyExpanded)
                    return node;

                node = node.bestChild();
            }
            return node;
        }
        private MCTSNode expansion(MCTSNode node) {
            if (!node.IsFullyExpanded)
                return node.expand();

            return node;
        }
        private int simulation(MCTSNode node) {
            GameState currentState = node.State;
            int MAX_SEARCH = 10;
            int i = 0;
            while (!currentState.isGameOver() && i < MAX_SEARCH)
            {
                List<GameMove> possibleMoves = currentState.getPossibleMoves();
                var randomMove = possibleMoves[random.Next(possibleMoves.Count)];
                currentState = currentState.applyMove(randomMove);
                i++;
            }
            return currentState.getWinner();
        }
        private void backpropagation(MCTSNode node, int result) {
            while (node != null)
            {
                node.addResult(result);
                node = node.Parent;
            }
        }

    }

    public class MCTSNode {
        private MCTSNode parent;
        private List<MCTSNode> children = new List<MCTSNode>();
        private int wins;
        private int visits;
        private GameState state;
        public GameState State {get => state;}
        public MCTSNode Parent {get => parent;}
        public bool IsFullyExpanded => children.Count == state.Moves;
        public MCTSNode(MCTSNode parent, GameState state) {
            this.parent = parent;
            this.state = state;
        }

        public double UCT() {
            if (visits==0) {
                return double.MaxValue;
            }
            return (double) wins/visits + Mathf.Sqrt(2*Mathf.Log(parent.visits)/visits);
        }
        public void addResult(int result) {
            visits++;
            // result 1 is win, 0 loss
            wins += result;
        }
        public MCTSNode bestChild() {
            MCTSNode bestChild = null;
            double bestValue = double.MinValue;
            foreach (var child in children)
            {
                double uctValue = child.UCT();
                if (uctValue > bestValue)
                {
                    bestValue = uctValue;
                    bestChild = child;
                }
            }
            return bestChild;
        }
        public MCTSNode expand() {
            List<GameMove> possibleMoves = state.getPossibleMoves();
            List<GameMove> untriedMoves = new List<GameMove>();

            // Find untried moves
            foreach (var move in possibleMoves)
            {
                bool found = false;
                foreach (var child in children)
                {
                    if (child.state.LastMove.Equals(move))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    untriedMoves.Add(move);
            }
            if (untriedMoves.Count > 0)
            {
                var randomMove = untriedMoves[new System.Random().Next(untriedMoves.Count)];
                var newState = state.applyMove(randomMove);
                var newNode = new MCTSNode(this, newState);
                children.Add(newNode);
                return newNode;
            }
            return null;
        }

    }


}

