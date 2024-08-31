using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions.Script;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Actions;
using System.Threading.Tasks;

namespace Creatures {

    public class CreatureActionCollection {
        public List<ScriptedAction> actions;

        public CreatureActionCollection(List<ScriptedAction> actions)
        {
            this.actions = actions;
        }
    }

    public class CreatureActionHandle : RetrievedHandle<CreatureActionCollection>
    {
        public CreatureActionHandle(AsyncOperationHandle<IList<ScriptableObject>> handle) : base(handle)
        {
        }

        protected override CreatureActionCollection setValue(AsyncOperationHandle<IList<ScriptableObject>> handle)
        {
            List<ScriptedAction> actions = new List<ScriptedAction>();
            foreach (var obj in handle.Result)
            {
                ScriptedAction action = obj as ScriptedAction;
                if (action != null)
                {
                    actions.Add(action);
                }
            }
            return new CreatureActionCollection(actions);
        }
    }
}

