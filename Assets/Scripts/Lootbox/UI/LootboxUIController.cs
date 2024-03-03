using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LootBoxModule {
    public class LootboxUIController : MonoBehaviour
    {
        private GameObject panelPrefab;
        public void Start() {
            panelPrefab = Resources.Load<GameObject>("UI/Lootbox/LootboxPanel");
        }
        [SerializeField] public GridLayoutGroup layoutGroup;
        // Start is called before the first frame update
        public void init(List<LootboxCount> lootBoxes) {
            foreach (LootboxCount lootboxCount in lootBoxes) {
                loadLootbox(lootboxCount);
            }
        }

        private void loadLootbox(LootboxCount lootboxCount) {
            GameObject panel = GameObject.Instantiate(panelPrefab);
            LootBoxPanel lootBoxPanel = panel.GetComponent<LootBoxPanel>();
            lootBoxPanel.set(lootboxCount);
            
        }
    }

}
