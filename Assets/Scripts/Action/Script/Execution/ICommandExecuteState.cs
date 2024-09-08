using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;

namespace Actions.Script.Execution {
    public interface ICommandExecutionState
    {
        public ICombatCreature getSelfCreature();
        public ICombatCreature getTargetCreature();
        public ICombatCreature getCreatureFromIndicator(FormattedScriptCommand scriptCommand, string objIndicator) {
            switch (objIndicator) {
                case "self":
                    return getSelfCreature();
                case "target":
                    return getTargetCreature();
                default:
                    ActionScriptInterpretorUtils.scriptError(scriptCommand,$"{objIndicator} is not a valid object indiciator");
                    return null;
            }
        }
        public ICombatCreature getActionTarget(bool self) {
            if (self) {
                return getSelfCreature();
            } else {
                return getTargetCreature();
            }
        }
        public Stack<ScriptCommand> getStack();
        public SelectionCommandLoop getSubStack();
        public EquipmentActionExecutor getEquipmentActionExecutor();
    }
}

