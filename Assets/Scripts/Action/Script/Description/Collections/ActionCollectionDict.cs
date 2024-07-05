using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions.Script {
    public class ActionDescriptionCollectionDict
    {
        private ActionTargetType actionTargetType;
        private Dictionary<string, ActionDescriptionCollection> actionDict;
        public ActionDescriptionCollectionDict(ActionTargetType actionTargetType)
        {
            this.actionTargetType = actionTargetType;
            actionDict = new Dictionary<string, ActionDescriptionCollection>();
        }

        public bool isTargetType(ActionTargetType targetType) {
            return targetType == actionTargetType;

        }
        public void addCommand(ScriptCommand scriptCommand) {
            if (!actionDict.ContainsKey(scriptCommand.Command)) {
                ActionDescriptionCollection actionDescriptionCollection = ScriptDescriptionCollectionUtils.getActionCollection(
                    scriptCommand.Command,
                    actionTargetType
                );
                if (actionDescriptionCollection == null) {
                    return;
                }
                actionDict[scriptCommand.Command] = actionDescriptionCollection;
            }
            actionDict[scriptCommand.Command].addCommand(scriptCommand);
        }

        public string getDescription(List<string> actionOrder, bool passiveVoice) {
            int count = getCount();
            bool singular = count == 1;
            string description = "";
            foreach (string action in actionOrder) {
                if (!actionDict.ContainsKey(action)) {
                    continue;
                }
                ActionDescriptionCollection collection = actionDict[action];
                List<string> actions = collection.getDescription();
                if (actions.Count == 0) {
                    continue;
                }
                string prefix = collection.getPrefix(passiveVoice);
                string suffix = collection.getSuffix(passiveVoice);
                bool prefixContainsAnd = count == 1;
                string actionDescription = !prefixContainsAnd || singular ? $"{prefix} " : $" and {prefix} ";
                foreach (string individualDescription in actions) {
                    if (count > 1) {
                        actionDescription += $"{individualDescription}, ";
                    } else {
                        if (prefixContainsAnd || singular) {
                            actionDescription += $"{individualDescription}";
                        } else {
                            actionDescription += $"and {individualDescription}";
                        }
                        
                    }
                    count--;
                }
                description += $"{actionDescription}{suffix}";
            }
            return description;
        }

        private int getCount() {
            int count = 0;
            foreach (string val in actionDict.Keys) {
                List<string> actions = actionDict[val].getDescription();
                count += actions.Count;
            }
            return count;
        }
    }
}

