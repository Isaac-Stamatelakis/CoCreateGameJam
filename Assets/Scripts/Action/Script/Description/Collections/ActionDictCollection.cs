using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Actions.Script.Description {
    public class ActionDictCollection {
        private string prefix;
        private string suffix;
        private bool passiveVoice;
        private List<ActionTargetType> validTargets;
        private List<ActionDescriptionCollectionDict> actionDescriptionCollections;
        public ActionDictCollection(string prefix, string suffix, List<ActionTargetType> validTargets, bool passiveVoice)
        {
            actionDescriptionCollections = new List<ActionDescriptionCollectionDict>();
            this.validTargets = validTargets;
            this.prefix = prefix;
            this.suffix = suffix;
            this.passiveVoice = passiveVoice;
        }
        public void addActionDescription(FormattedScriptCommand formattedScriptCommand, ActionTargetType targetType) {
            if (!validTargets.Contains(targetType)) {
                throw new Exception($"{targetType} is not a valid target in this collection");
            }
            ActionDescriptionCollectionDict actionDescriptionCollection = null;
            if (actionDescriptionCollections.Count == 0 || !actionDescriptionCollections.Last().isTargetType(targetType)) {
                actionDescriptionCollection = new ActionDescriptionCollectionDict(targetType);
                actionDescriptionCollections.Add(actionDescriptionCollection);
            } else {
                actionDescriptionCollection = actionDescriptionCollections.Last();
            }
            actionDescriptionCollection.addCommand(formattedScriptCommand);
        }

        public string getDescription(List<string> actionOrder) {
            if (actionDescriptionCollections.Count == 0) {
                return null;
            }
            string description = $"{prefix}";
            for (int i = 0; i < actionDescriptionCollections.Count; i++) {
                ActionDescriptionCollectionDict collectionDict = actionDescriptionCollections[i];
                string actionDescritionCollection = collectionDict.getDescription(actionOrder,passiveVoice);
                if (actionDescritionCollection == null) {
                    continue;
                }
                bool isLast = i == actionDescriptionCollections.Count-1;
                if (isLast) {
                    description += actionDescritionCollection;
                } else {
                    description += $"{actionDescritionCollection}.";
                }
            }
            if (suffix == null) {
                return $"{description}.";
            } else {
                return $"{description} {suffix}.";
            }
        }
    }
}