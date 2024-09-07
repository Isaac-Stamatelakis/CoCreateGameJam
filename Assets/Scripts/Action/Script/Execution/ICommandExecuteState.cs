using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;

namespace Actions.Script.Execution {
    public interface ICommandExecuteState
    {
        public CreatureInCombat getSelfCreature();
        public CreatureInCombat getTargetCreature();
    }
}

