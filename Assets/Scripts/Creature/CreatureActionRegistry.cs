using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions.Script;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Actions;
using System.Threading.Tasks;

namespace Creatures {
    public class CreatureActionRegistry
    {
        private static CreatureActionRegistry instance;
        private Dictionary<string,CreatureActionHandle> actions;
        public int Count {get => actions.Count;}
        private CreatureActionRegistry() {
            actions = new Dictionary<string, CreatureActionHandle>();
        }
        public static CreatureActionRegistry getInstance() {
            if (instance == null) {
                instance = new CreatureActionRegistry();
            }
            return instance;
        }
        public CreatureActionCollection getAction(string id) {
            return actions.ContainsKey(id) ? actions[id].Value : null;
        }
        public async Task loadActions(string id) {
            if (actions.ContainsKey(id)) {
                return;
            }
            AsyncOperationHandle<IList<ScriptableObject>> handle = Addressables.LoadAssetsAsync<ScriptableObject>(id, null);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                CreatureActionHandle retrieveHandle = new CreatureActionHandle(handle);
                actions[id] = retrieveHandle;
            }
        }

        public void free() {
            foreach (CreatureActionHandle creatureActionHandle in actions.Values) {
                creatureActionHandle.free();
            }
            actions = new Dictionary<string, CreatureActionHandle>();
        }
    }

    

    public class CreatureActionCollection {
        public List<ScriptedAction> actions;

        public CreatureActionCollection(List<ScriptedAction> actions)
        {
            Debug.Log(actions.Count);
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

