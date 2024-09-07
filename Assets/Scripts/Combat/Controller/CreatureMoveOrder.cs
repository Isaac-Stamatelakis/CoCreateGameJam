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
            addPlayer(humanPlayer);
            addPlayer(aiPlayer);
            creatureTurns = creatureTurns.OrderByDescending(creature => creature.getStat(CreatureStat.Speed)).ToList();
        }

        private void addPlayer(CombatPlayer combatPlayer) {
            creatureTurns.AddRange(combatPlayer.Creatures);
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
                CreatureInCombat creatureInCombat = creatureCombatDict[creatures[i]];
                if (creatureInCombat.IsDead) {
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

        public CreatureCombatObject getCurrentCreatureObject() {
            CreatureInCombat currentlyMoving = getCurrentCombatCreature();
            if (currentlyMoving == null) {
                return null;
            }
            return currentlyMoving.CreatureCombatObject;
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
        public CreatureInCombat getRandomCreature(bool includeCurrentlyMoving) {
            if (includeCurrentlyMoving && creatureTurns.Count == 1) {
                return null;
            }
            int startIndex = includeCurrentlyMoving ? 0 : 1;
            int rand = Random.Range(startIndex,creatureTurns.Count);
            return getCombatCreature(creatureTurns[rand]);
        }

        public List<CreatureInCombat> getSelectableCreatures(CreatureSelectionType targetType, bool random, bool targetSelf) {
            return new List<CreatureInCombat>();
        }

        public bool isFirstPlayersTurn() {
            EquipedCreature creature = getCurrentCreature();
            if (humanPlayer.Creatures.Count <= aiPlayer.Creatures.Count) {
                return humanPlayer.HasCreature(creature);
            } else {
                return aiPlayer.HasCreature(creature);
            }
        }
        public bool isSecondPlayersTurn() {
            return !isFirstPlayersTurn();
        }
    }
}

