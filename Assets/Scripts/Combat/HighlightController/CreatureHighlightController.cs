using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions;

namespace Levels.Combat {
    public class CreatureHighlightController
    {
        private CreatureSelector creatureSelector;
        private CreatureCombatObject currentlyMovingCreature;
        private CreatureCombatObject viewHighlightedCreature;
        private CombatLevelDisplayedCreatureUI displayedCreatureUI;
        private CombatPlayer humanPlayer;
        private List<CreatureCombatObject> previouslyHighlightedEnemies;
        private List<CreatureCombatObject> previouslyHighlightedAllies;
        public CreatureHighlightController(CombatPlayer humanPlayer, CombatLevelDisplayedCreatureUI displayedCreatureUI) {
            this.humanPlayer = humanPlayer;
            this.displayedCreatureUI = displayedCreatureUI;
        }
        public void highlight(CreatureCombatObject creatureCombatObject) {
            if (creatureSelector == null) {
                highlightForView(creatureCombatObject);
            } else {
                highlightForSelector(creatureCombatObject);
            }
            
        }
        private void highlightForView(CreatureCombatObject creatureCombatObject) {
            bool selectedCurrentlyMovingCreature = creatureCombatObject.Equals(currentlyMovingCreature);
            bool selectedCurrentlyHighlightedCreature = creatureCombatObject.Equals(viewHighlightedCreature);
            if (selectedCurrentlyMovingCreature || selectedCurrentlyHighlightedCreature) {
                unhighlightCreature(viewHighlightedCreature);
                viewHighlightedCreature = null;
                return;
            }
            if (viewHighlightedCreature != null) {
                viewHighlightedCreature.highlight(null);
            }
            viewHighlightedCreature = creatureCombatObject;
            viewHighlightedCreature.highlight(HighlightType.View);
            displayedCreatureUI.display(viewHighlightedCreature.CreatureInCombat);
        }

        private void highlightForSelector(CreatureCombatObject creatureCombatObject) {
            if (creatureCombatObject == null || creatureCombatObject.CreatureInCombat.IsDead) {
                return;
            }
            bool isAlly = humanPlayer.Creatures.Contains(creatureCombatObject.CreatureInCombat);
            bool isSelf = creatureCombatObject.Equals(currentlyMovingCreature);
            bool validSelection = creatureSelector.isValidSelection(isAlly,isSelf);
            if (!validSelection) {
                return;
            }
            bool selected = creatureSelector.Creatures.Contains(creatureCombatObject);
            if (selected) {
                unhighlightCreature(creatureCombatObject);
                creatureSelector.Creatures.Remove(creatureCombatObject);
                creatureSelector.updateDescription();
                return;
            }
            if (creatureSelector.Creatures.Count >= creatureSelector.MaxTargets) {
                CreatureCombatObject firstInRemovalQueue = creatureSelector.Creatures[0];
                firstInRemovalQueue.highlight(null);
                creatureSelector.Creatures.Remove(firstInRemovalQueue);
                creatureSelector.updateDescription();
            }
            creatureSelector.Creatures.Add(creatureCombatObject);
            if (isAlly) {
                creatureCombatObject.highlight(HighlightType.Ally);
            } else {
                creatureCombatObject.highlight(HighlightType.Enemy);
            }
            creatureSelector.updateDescription();
        }
        private void unhighlightCreature(CreatureCombatObject creatureCombatObject) {
            if (creatureCombatObject==null) {
                return;
            }
            bool selectedViewHighlightedCreature = creatureCombatObject.Equals(viewHighlightedCreature);
            if (selectedViewHighlightedCreature) {
                viewHighlightedCreature = null;
                displayedCreatureUI.display(currentlyMovingCreature.CreatureInCombat);
            }
            creatureCombatObject.highlight(null);
            
        }

        public void setSelector(CreatureSelector newSelector) {
            if (this.creatureSelector != null) {
                List<CreatureCombatObject> creatureCombatObjects = this.creatureSelector.Creatures;
                List<CreatureCombatObject> enemyCreatures = new List<CreatureCombatObject>();
                List<CreatureCombatObject> allyCreatures = new List<CreatureCombatObject>();
                foreach (CreatureCombatObject creatureCombatObject in creatureCombatObjects) {
                    if (humanPlayer.Creatures.Contains(creatureCombatObject.CreatureInCombat)) {
                        allyCreatures.Add(creatureCombatObject);
                    } else {
                        enemyCreatures.Add(creatureCombatObject);
                    }
                }
                if (allyCreatures.Count > 0) {
                    previouslyHighlightedAllies = allyCreatures;
                }
                if (enemyCreatures.Count > 0) {
                    previouslyHighlightedEnemies = enemyCreatures;
                }
                this.creatureSelector.clear();
            }
            this.creatureSelector = newSelector;
            if (newSelector != null) {
                selectFromMemory(new List<CreatureCombatObject>{viewHighlightedCreature});
                selectFromMemory(previouslyHighlightedEnemies);
                selectFromMemory(previouslyHighlightedAllies);
            }
        }

        private void selectFromMemory(List<CreatureCombatObject> creatureCombatObjects) {
            if (creatureCombatObjects == null) {
                return;
            }
            foreach (CreatureCombatObject creatureCombatObject in creatureCombatObjects) {
                if (creatureSelector.IsFull) {
                    return;
                }
                highlightForSelector(creatureCombatObject);
            }
        }
        

        public void setCurrentCreatureTurn(CreatureCombatObject creatureCombatObject) {
            if (currentlyMovingCreature != null) {
                currentlyMovingCreature.highlight(null);
            }
            currentlyMovingCreature = creatureCombatObject;
            currentlyMovingCreature.highlight(HighlightType.Turn);
            displayedCreatureUI.display(creatureCombatObject.CreatureInCombat);
        }
    }
}

