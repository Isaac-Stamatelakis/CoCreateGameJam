using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions.Script.Description {
    public interface IDescriptableCommand : IScriptCommand  {
        public void execute(ActionScriptDescriptionParser actionScriptDescriptionParser);
    }
}

