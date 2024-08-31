using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions.Script;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Actions;
using System.Threading.Tasks;

namespace Creatures {
    public class EquipmentActionRegistry
    {
        private static EquipmentActionRegistry instance;
        private Dictionary<string,EquipmentActionHandle> actions;
        public int Count {get => actions.Count;}
        private EquipmentActionRegistry() {
            actions = new Dictionary<string, EquipmentActionHandle>();
        }
        public static EquipmentActionRegistry getInstance() {
            if (instance == null) {
                instance = new EquipmentActionRegistry();
            }
            return instance;
        }
        public EquipmentActionCollection getAction(string id) {
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
                EquipmentActionHandle retrieveHandle = new EquipmentActionHandle(handle);
                actions[id] = retrieveHandle;
            }
        }

        public void free() {
            foreach (EquipmentActionHandle creatureActionHandle in actions.Values) {
                creatureActionHandle.free();
            }
            actions = new Dictionary<string, EquipmentActionHandle>();
        }
    }

    

    public class EquipmentActionCollection {
        public List<ScriptedAction> actions;
        public ScriptedAction onDamaged;
        public ScriptedAction onHealed;
        public ScriptedAction onAttack;

        public EquipmentActionCollection(List<ScriptedAction> actions, ScriptedAction onDamaged, ScriptedAction onHealed, ScriptedAction onAttack)
        {
            this.actions = actions;
            this.onDamaged = onDamaged;
            this.onHealed = onHealed;
            this.onAttack = onAttack;
        }
    }

    public class EquipmentActionHandle : RetrievedHandle<EquipmentActionCollection>
    {
        public EquipmentActionHandle(AsyncOperationHandle<IList<ScriptableObject>> handle) : base(handle)
        {
        }

        protected override EquipmentActionCollection setValue(AsyncOperationHandle<IList<ScriptableObject>> handle)
        {
            List<ScriptedAction> actions = new List<ScriptedAction>();
            ScriptedAction onHeal = null;
            ScriptedAction onDamaged = null;
            ScriptedAction onAttack = null;
            if (handle.Result.Count == 0) {
                return null;
            }
            foreach (var obj in handle.Result)
            {
                ScriptedAction action = obj as ScriptedAction;
                if (action != null)
                {
                    switch (action.ActionType) {
                        case ScriptedActionType.Standard:
                            actions.Add(action);
                            break;
                        case ScriptedActionType.OnDamaged:
                            setSpecialAction(ref onDamaged,action);
                            break;
                        case ScriptedActionType.OnAttack:
                            setSpecialAction(ref onAttack,action);
                            break;
                        case ScriptedActionType.OnHealed:
                            setSpecialAction(ref onHeal,action);
                            break;
                    }

                }
            }
            return new EquipmentActionCollection(
                actions,
                onDamaged,
                onHeal,
                onAttack
            );
        }

        private void setSpecialAction(ref ScriptedAction toSet, ScriptedAction value) {
            if (toSet != null) {
                Debug.LogWarning($"There is more than one {value.ActionScript} action. Overriding {toSet.name} with {value.name}");
            }
            toSet = value;
        }
    }
}

