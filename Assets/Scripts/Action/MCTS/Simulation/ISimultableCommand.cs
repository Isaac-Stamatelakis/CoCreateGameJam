using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions.Script.Execution;

namespace Actions.Script {
    public interface ISimultableCommand
    {
        public void execute(SimulatedExecutionState state);
    }
}

