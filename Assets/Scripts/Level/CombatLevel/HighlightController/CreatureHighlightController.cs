using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures.Actions;

namespace Levels.Combat {
    public class CreatureHighlightController
    {
        private CreatureSelector creatureSelector;
        private CreatureCombatObject currentlyMovingCreature;
        private CreatureCombatObject viewHighlightedCreature;
        private CombatLevelDisplayedCreatureUI displayedCreatureUI;
        private CombatPlayer humanPlayer;
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
            bool isAlly = humanPlayer.Creatures.Contains(creatureCombatObject.CreatureInCombat);
            bool isSelf = creatureCombatObject.Equals(currentlyMovingCreature);
            bool validSelection = isValidSelection(isAlly,isSelf);
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
            bool selectedCurrentlyMovingCreature = creatureCombatObject.Equals(currentlyMovingCreature);
            if (selectedCurrentlyMovingCreature) {
                currentlyMovingCreature.highlight(HighlightType.Turn);
                displayedCreatureUI.display(viewHighlightedCreature.CreatureInCombat);
            } else {
                creatureCombatObject.highlight(null);
            }
        }

        public void setSelector(CreatureSelector creatureSelector) {
            if (this.creatureSelector != null) {
                this.creatureSelector.clear();
            }
            this.creatureSelector = creatureSelector;
        }

        private bool isValidSelection(bool isAlly, bool isSelf) {
            switch (creatureSelector.TargetType) {
                case TargetType.Ally:
                    if (!isAlly) {
                        return false;
                    }
                    break;
                case TargetType.Enemy:
                    if (isAlly) {
                        return false;
                    }
                    break;
                case TargetType.Any_Not_Self:
                    if (!isSelf) {
                        return false;
                    }
                    break;
                case TargetType.Ally_Not_Self:
                    if (!isAlly || isSelf) {
                        return false;
                    }
                    break;
            }
            return true;
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

