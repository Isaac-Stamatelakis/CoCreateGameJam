using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using System.Linq;
using Actions.Script;
using Actions;

namespace Levels.Combat {
    
    public class CreatureMoveOrder
    {
        private List<EquipedCreature> creatureTurns;
        public List<EquipedCreature> Creatures {get => creatureTurns;}
        private CombatPlayer humanPlayer;
        private CombatPlayer aiPlayer;
        public bool IsEmpty {get => creatureTurns.Count==0;}
        private Dictionary<EquipedCreature,CreatureInCombat> creatureCombatDict;
        public CreatureMoveOrder(CombatPlayer humanPlayer, CombatPlayer aiPlayer)
        {
            creatureTurns = new List<EquipedCreature>();
            creatureCombatDict = new Dictionary<EquipedCreature, CreatureInCombat>();
            
            creatureTurns.AddRange(aiPlayer.Creatures);
            this.humanPlayer = humanPlayer;
            this.aiPlayer = aiPlayer;
            addPlayerToDict(humanPlayer);
            creatureTurns.AddRange(humanPlayer.Creatures);
            addPlayerToDict(aiPlayer);
            creatureTurns.AddRange(aiPlayer.Creatures);
            creatureTurns = creatureTurns.OrderByDescending(creature => creature.getStat(CreatureStat.Speed)).ToList();
        }

        public CreatureMoveOrder(CombatPlayer humanPlayer, CombatPlayer aiPlayer, List<EquipedCreature> turns, Dictionary<EquipedCreature,CreatureInCombat> dict) {
            this.humanPlayer = humanPlayer;
            this.aiPlayer = aiPlayer;
            creatureTurns = turns;
            this.creatureCombatDict = dict;
        }

        private void addPlayerToDict(CombatPlayer combatPlayer) {
            
            foreach (EquipedCreature equipedCreature in combatPlayer.Creatures) {
                creatureCombatDict[equipedCreature] = new CreatureInCombat(equipedCreature);
            }
        }

        public void clearDeadCreatures() {
            clearDeadCreatureList(creatureTurns);
            clearDeadCreatureList(humanPlayer.Creatures);
            clearDeadCreatureList(aiPlayer.Creatures);
        }

        private void clearDeadCreatureList(List<EquipedCreature> creatures) {
            for (int i = 0; i < creatures.Count; i++) {
                if (!creatureCombatDict.ContainsKey(creatures[i])) {
                    creatures.RemoveAt(i);
                    i--;
                }
                if (i < 0) {
                    break;
                }
                CreatureInCombat creatureInCombat = creatureCombatDict[creatures[i]];
                if (creatureInCombat.IsDead) {
                    creatureCombatDict.Remove(creatures[i]);
                    creatures.RemoveAt(i);
                    i--;
                }
            }
        }

        public List<CreatureInCombat> getCombatCreatures(CombatPlayer combatPlayer) {
            List<CreatureInCombat> combatCreatures = new List<CreatureInCombat>();
            foreach (EquipedCreature equipedCreature in combatPlayer.Creatures) {
                combatCreatures.Add(creatureCombatDict[equipedCreature]);
            }
            return combatCreatures;
        }

        public void changeTurn() {
            EquipedCreature currentTurn = creatureTurns[0];
            creatureTurns.RemoveAt(0);
            creatureTurns.Add(currentTurn);
        }

        public bool isGameOver() {
            return humanPlayer.IsDead() || aiPlayer.IsDead();
        }

        public CreatureInCombat getCombatCreature(EquipedCreature equipedCreature) {
            return creatureCombatDict[equipedCreature];
        }
        public EquipedCreature getCurrentCreature() {
            if (creatureTurns.Count == 0) {
                return null;
            }
            return creatureTurns[0];
        }
        public CreatureInCombat getCurrentCombatCreature() {
            if (creatureTurns.Count == 0) {
                return null;
            }
            return getCombatCreature(creatureTurns[0]);
        }
        public EquipedCreature getRandomCreature(CreatureSelectionType targetType, bool includeCurrentlyMoving) {
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

        private EquipedCreature sampleRandom(List<EquipedCreature> creatures, int exclusion) {
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

        

        public List<CreatureInCombat> getSelectableCreatures(CreatureSelectionType targetType, bool random, bool targetSelf) {
            List<CreatureInCombat> creatureInCombats = new List<CreatureInCombat>();
            bool addedSelf = false;
            if (targetType == CreatureSelectionType.Ally || targetType == CreatureSelectionType.Any) {
                addedSelf = true;
                if (isHumanPlayerTurn()) {
                    creatureInCombats.AddRange(getCombatCreatures(humanPlayer));
                } else {
                    creatureInCombats.AddRange(getCombatCreatures(aiPlayer));
                }
            }
            if (targetType == CreatureSelectionType.Enemy || targetType == CreatureSelectionType.Any) {
                if (isHumanPlayerTurn()) {
                    creatureInCombats.AddRange(getCombatCreatures(aiPlayer));
                } else {
                    creatureInCombats.AddRange(getCombatCreatures(humanPlayer));
                }
            }
            if (!targetSelf && addedSelf) {
                CreatureInCombat selfCreature = getCurrentCombatCreature();
                creatureInCombats.Remove(selfCreature);
                
            }
            return creatureInCombats;
        }

        public bool isHumanPlayerTurn() {
            EquipedCreature creature = getCurrentCreature();
            if (humanPlayer.Creatures.Count <= aiPlayer.Creatures.Count) {
                return humanPlayer.HasCreature(creature);
            } else {
                return !aiPlayer.HasCreature(creature);
            }
        }
        public bool isSecondPlayersTurn() {
            return !isHumanPlayerTurn();
        }
        public CreatureMoveOrder deepCopy() {
            Dictionary<EquipedCreature, CreatureInCombat> copyDict = new Dictionary<EquipedCreature, CreatureInCombat>();
            foreach (var kvp in creatureCombatDict) {
                copyDict[kvp.Key] = kvp.Value.deepCopy();
            } 
            return new CreatureMoveOrder(humanPlayer,aiPlayer,creatureTurns,copyDict);
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
            foreach (EquipedCreature equipedCreature in combatPlayer.Creatures) {
                health += getCombatCreature(equipedCreature).Health;
            }
            return health;
        }
    }
}

