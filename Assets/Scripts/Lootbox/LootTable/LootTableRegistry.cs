using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Creatures;
using Items.Equipment;
using System.Threading.Tasks;

namespace LootBoxes {
    public static class LootTableRegistry
    {
        public static async Task<LootTableObject> retrieve(string id) {
            if (!AddressableUtils.AddressableLabelExists(id)) {
                return null;
            }
            AsyncOperationHandle<IList<LootTableObject>> handle = Addressables.LoadAssetsAsync<LootTableObject>(id, null);

            await handle.Task;
            Addressables.Release(handle);
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                IList<LootTableObject> lootTableObjects = handle.Result;
                if (lootTableObjects.Count > 1) {
                    Debug.LogWarning($"{id} is used multiple times");
                }
                return lootTableObjects[0];
            } else {
                Debug.LogWarning($"Could not retrieve '{id}' due to {handle.OperationException}");
            } 

            return null;
        }
    }

}
