using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Actions {
    public class CreatureActionRegistry<T,V> where T : RetrievedHandle<V>
    {
        private static CreatureActionRegistry<T,V> instance;
        private Dictionary<string,T> actions;
        private CreatureActionRegistry() {
            actions = new Dictionary<string, T>();
        }
        public static CreatureActionRegistry<T,V> getInstance() {
            if (instance == null) {
                instance = new CreatureActionRegistry<T,V>();
            }
            return instance;
        }
        public V getAction(string id) {
            return actions.ContainsKey(id) ? actions[id].Value : default(V);
        }
        public async void loadActions(string id) {
            AsyncOperationHandle<IList<ScriptableObject>> handle = Addressables.LoadAssetsAsync<ScriptableObject>(id, null);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                T retrieveHandle = (T)System.Activator.CreateInstance(typeof(T), handle);
                actions[id] = retrieveHandle;
            }
        }
    }

    public abstract class RetrievedHandle<T> {
        private AsyncOperationHandle<IList<ScriptableObject>> handle;
        public RetrievedHandle(AsyncOperationHandle<IList<ScriptableObject>> handle) {
            this.handle = handle;
            value = setValue(handle);
        }
        private T value;
        protected abstract T setValue(AsyncOperationHandle<IList<ScriptableObject>> handle);
        public void free() {
            Addressables.Release(handle);
        }
        public T Value {get => value;}
    }
}

