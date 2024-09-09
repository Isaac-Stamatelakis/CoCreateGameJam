using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions.Script;
using Actions;
using Levels.Combat;
using Creatures;
using Actions.Script.Execution;
using System.Linq;
using Actions.MCTS;


namespace Levels.Combat {
    public class GameState
    {
        public int Moves;
        public GameMove LastMove {get; private set;} 
        private List<CreatureInCombat> creatureTurns;
        private CombatPlayer humanPlayer;
        private CombatPlayer aiPlayer;
        public bool IsEmpty {get => creatureTurns.Count==0;}
        public int AliveCreatures {get => creatureTurns.Count;}
        public CombatPlayer HumanPlayer { get => humanPlayer;}
        public CombatPlayer AiPlayer { get => aiPlayer; }

        public GameState(CombatPlayer humanPlayer, CombatPlayer aiPlayer)
        {
            creatureTurns = new List<CreatureInCombat>();
            this.humanPlayer = humanPlayer;
            this.aiPlayer = aiPlayer;
            creatureTurns.AddRange(humanPlayer.Creatures);
            creatureTurns.AddRange(aiPlayer.Creatures);
            creatureTurns = creatureTurns.OrderByDescending(creature => creature.getStat(CreatureStat.Speed)).ToList();
        }

        public GameState(CombatPlayer humanPlayer, CombatPlayer aiPlayer, List<CreatureInCombat> turns) {
            this.humanPlayer = humanPlayer;
            this.aiPlayer = aiPlayer;
            creatureTurns = turns;
        }

        public void clearDeadCreatures() {
            clearDeadCreatureList(creatureTurns);
            clearDeadCreatureList(humanPlayer.Creatures);
            clearDeadCreatureList(aiPlayer.Creatures);
        }

        private void clearDeadCreatureList(List<CreatureInCombat> creatures) {
            for (int i = 0; i < creatures.Count; i++) {
                CreatureInCombat creatureInCombat = creatures[i];
                if (creatureInCombat.IsDead) {
                    creatures.RemoveAt(i);
                    i--;
                }
            }
        }

        public void changeTurn() {
            CreatureInCombat currentTurn = creatureTurns[0];
            creatureTurns.RemoveAt(0);
            creatureTurns.Add(currentTurn);
            clearDeadCreatures();
        }

        public bool isGameOver() {
            return humanPlayer.IsDead() || aiPlayer.IsDead();
        }
        public bool humanWin() {
            return aiPlayer.IsDead() && !humanPlayer.IsDead();
        }
        public bool aiWin() {
            return !aiPlayer.IsDead() && humanPlayer.IsDead();
        }
        public bool tie() {
            return aiPlayer.IsDead() && humanPlayer.IsDead();
        }

        public CreatureInCombat getCurrentCreature() {
            if (creatureTurns.Count == 0) {
                return null;
            }
            return creatureTurns[0];
        }
        public CreatureInCombat getRandomCreature(CreatureSelectionType targetType, bool includeCurrentlyMoving) {
            if (includeCurrentlyMoving && creatureTurns.Count == 1) {
                return null;
            }
            if (targetType == CreatureSelectionType.Any) {
                int exclusion = includeCurrentlyMoving ? -1 : 0;
                return sampleRandom(creatureTurns,exclusion);
            }
            bool humanTurn = isHumanPlayerTurn();
            if (humanTurn) {
                if (targetType == CreatureSelectionType.Ally) {
                    int exclusion = includeCurrentlyMoving ? -1 : humanPlayer.Creatures.IndexOf(creatureTurns[0]);
                    return sampleRandom(humanPlayer.Creatures,exclusion);
                } else if (targetType == CreatureSelectionType.Enemy) {
                    return sampleRandom(aiPlayer.Creatures,-1);
                }
            } else {
                if (targetType == CreatureSelectionType.Ally) {
                    int exclusion = includeCurrentlyMoving ? -1 : aiPlayer.Creatures.IndexOf(creatureTurns[0]);
                    return sampleRandom(aiPlayer.Creatures,exclusion);
                } else if (targetType == CreatureSelectionType.Enemy) {
                    return sampleRandom(humanPlayer.Creatures,-1);
                }
            }
            return null;
        }

        private CreatureInCombat sampleRandom(List<CreatureInCombat> creatures, int exclusion) {
            int rand;
            if (exclusion == 0) {
                rand = Random.Range(1,creatures.Count);
                return creatures[rand];
            } 
            do {
                rand = Random.Range(0, creatures.Count);
            } while (rand == exclusion);
            return creatures[rand];
        }
        public List<CreatureInCombat> getSelectableCreatures(CreatureSelectionType targetType, bool targetSelf) {
            List<CreatureInCombat> creatureInCombats = new List<CreatureInCombat>();
            bool addedSelf = false;
            if (targetType == CreatureSelectionType.Ally || targetType == CreatureSelectionType.Any) {
                addedSelf = true;
                if (isHumanPlayerTurn()) {
                    creatureInCombats.AddRange(humanPlayer.Creatures);
                } else {
                    creatureInCombats.AddRange(aiPlayer.Creatures);
                }
            }
            if (targetType == CreatureSelectionType.Enemy || targetType == CreatureSelectionType.Any) {
                if (isHumanPlayerTurn()) {
                    creatureInCombats.AddRange(aiPlayer.Creatures);
                } else {
                    creatureInCombats.AddRange(humanPlayer.Creatures);
                }
            }
            if (!targetSelf && addedSelf) {
                CreatureInCombat selfCreature = getCurrentCreature();
                creatureInCombats.Remove(selfCreature);
            }
            return creatureInCombats;
        }

        public bool isHumanPlayerTurn() {
            CreatureInCombat creature = getCurrentCreature();
            return humanPlayer.HasCreature(creature);
            /*
            if (humanPlayer.Creatures.Count <= aiPlayer.Creatures.Count) {
                return humanPlayer.HasCreature(creature);
            } else {
                return !aiPlayer.HasCreature(creature);
            }
            */
        }
        public bool isSecondPlayersTurn() {
            return !isHumanPlayerTurn();
        }
        public GameState deepCopy() {
            List<CreatureInCombat> playerCreatureClone = new List<CreatureInCombat>();
            List<CreatureInCombat> aiCreatureClone = new List<CreatureInCombat>();
            List<CreatureInCombat> newTurns = new List<CreatureInCombat>();
            foreach (CreatureInCombat creatureInCombat in creatureTurns) {
                CreatureInCombat copy = creatureInCombat.deepCopy();
                newTurns.Add(copy);
                if (humanPlayer.HasCreature(creatureInCombat)) {
                    playerCreatureClone.Add(copy);
                } else {
                    aiCreatureClone.Add(copy);
                }
            }
            CombatPlayer humanCopy = new CombatPlayer(playerCreatureClone);
            CombatPlayer aiCopy = new CombatPlayer(aiCreatureClone);
            return new GameState(humanCopy,aiCopy,newTurns);
        }
        public bool isHumanWinning() {
            if (humanPlayer.IsDead()) {
                return false;
            }
            if (aiPlayer.IsDead()) {
                return true;
            }
            float totalHumanHealth = getPlayerHealth(humanPlayer);
            float totalAiHealth = getPlayerHealth(aiPlayer);
            return totalHumanHealth > totalAiHealth;
        }

        private float getPlayerHealth(CombatPlayer combatPlayer) {
            float health = 0;
            foreach (CreatureInCombat creatureInCombat in combatPlayer.Creatures) {
                health += creatureInCombat.getHealth();
            }
            return health;
        }

        public List<GameMove> getPossibleMoves() {
            string id = getCurrentCreature().EquipedCreeture.Creeture.getId();
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
                (int targets, CreatureSelectionType targetType, bool random, bool targetSelf, float? period) = SelectCommand.parse(selectCommand.getFormattedScriptCommand());
                List<CreatureInCombat> selectableCreatures = getSelectableCreatures(targetType,targetSelf);
                if (random) {
                    return new List<GameMove>{
                        new GameMove(
                            scriptedAction,
                            new SimulatedRandomSelector<CreatureInCombat>(selectableCreatures,targets)
                        )
                    };
                }
                // This assumes choosing max targets is always optimal (which is most likely is)
                List<List<CreatureInCombat>> choicePermutations = GameStateUtils.GeneratePermutations<CreatureInCombat>(selectableCreatures,targets);
                List<GameMove> moves = new List<GameMove>();
                foreach (List<CreatureInCombat> permutation in choicePermutations) {
                    moves.Add(new GameMove(scriptedAction,new CreatureSelector<CreatureInCombat>(permutation)));
                }
                return moves;
            }
            Debug.LogWarning($"{scriptedAction.name} did not have a select command");
            return new List<GameMove>();
        }
        public GameState simulateMove(GameMove gameMove) {
            GameState simulatedState = deepCopy();
            SimulatedExecutionState simulatedExecutionState = new SimulatedExecutionState(
                ScriptCommandFactory.parseCommands(gameMove.ScriptedAction.ActionScript),
                simulatedState.getCurrentCreature(),
                true,
                gameMove.Selector
            );
            LastMove = gameMove;
            Moves++;
            simulatedExecutionState.execute();
            simulatedState.changeTurn();
            return simulatedState;
        }
        public int getWinner() {
            // Game State is always from AI Perspective
            if (isHumanWinning()) {
                return 0; // HUMAN WINNING
            }
            return 1; // AI WINNING
        }
    }

    public class GameMove {
        private ScriptedAction scriptedAction;
        private CreatureSelector<CreatureInCombat> selector;
        public GameMove(ScriptedAction scriptedAction,  CreatureSelector<CreatureInCombat> selector)
        {
            this.scriptedAction = scriptedAction;
            this.selector = selector;
        }

        public ScriptedAction ScriptedAction { get => scriptedAction; }
        public CreatureSelector<CreatureInCombat> Selector { get => selector;}
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
