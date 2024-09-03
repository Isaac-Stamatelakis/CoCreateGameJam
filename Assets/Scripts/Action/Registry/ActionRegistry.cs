using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Creatures;
using Items.Equipment;
using System.Threading.Tasks;

namespace Actions {
    public class ActionRegistry
    {
        private static ActionRegistry instance;
        private Dictionary<string,IRetrivedHandle> actions;
        private ActionRegistry() {
            actions = new Dictionary<string, IRetrivedHandle>();
        }
        public static ActionRegistry getInstance() {
            if (instance == null) {
                instance = new ActionRegistry();
            }
            return instance;
        }
        public T getAction<T>(string id) {
            if (id == null || !actions.ContainsKey(id)) {
                return default(T);
            }
            if (actions[id].getValue() is T value) {
                return value;
            }
            return default(T);
        }

        public void freeAll() {
            foreach (IRetrivedHandle handle in actions.Values) {
                handle.free();
            }
            actions = new Dictionary<string, IRetrivedHandle>();
        }

        public void free<T>() {
            List<string> idsToRemove = new List<string>();
            foreach (KeyValuePair<string,IRetrivedHandle> kvp in actions) {
                IRetrivedHandle handle = kvp.Value;
                string id = kvp.Key;
                if (handle.getValue() is T) {
                    handle.free();
                    idsToRemove.Add(id);
                }
            }
            foreach (string id in idsToRemove) {
                actions.Remove(id);
            }

        }
        public async Task loadActions(string id, ActionBundleType actionBundleType) {
            if (actions.ContainsKey(id)) {
                return;
            }

            AsyncOperationHandle<IList<ScriptableObject>> handle = Addressables.LoadAssetsAsync<ScriptableObject>(id, null);
            await handle.Task;
            if (handle.OperationException is InvalidKeyException e) {
                Debug.LogWarning($"Action Registry could not load {actionBundleType} for id {id}\nError:{e}");
                return;
            }
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                IRetrivedHandle retrieveHandle = ActionHandleFactory.formatHandle(handle,actionBundleType);
                actions[id] = retrieveHandle;
            }
            
                
            
            
            
        }
    }

    public static class ActionHandleFactory {
        public static IRetrivedHandle formatHandle(AsyncOperationHandle<IList<ScriptableObject>> handle, ActionBundleType actionBundleType) {
            switch (actionBundleType) {
                case ActionBundleType.Creature:
                    return new CreatureActionHandle(handle);
                case ActionBundleType.Equipment:
                    return new EquipmentActionHandle(handle);
                default:
                    throw new System.Exception($"ActionHandleFactory did not cover case for {actionBundleType}");
            }
        }
    }
    public enum ActionBundleType {
        Creature,
        Equipment
    }
    public interface IRetrivedHandle {
        public object getValue();
        public void free();
    }

    public abstract class RetrievedHandle<T> : IRetrivedHandle {
        protected AsyncOperationHandle<IList<ScriptableObject>> handle;
        public RetrievedHandle(AsyncOperationHandle<IList<ScriptableObject>> handle) {
            this.handle = handle;
            value = setValue(handle);
        }
        private T value;
        protected abstract T setValue(AsyncOperationHandle<IList<ScriptableObject>> handle);
        public void free() {
            Addressables.Release(handle);
        }

        public object getValue()
        {
            return value;
        }

        public T Value {get => value;}
    }
}

