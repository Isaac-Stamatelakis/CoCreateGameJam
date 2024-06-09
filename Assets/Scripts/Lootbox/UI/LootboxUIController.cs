using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LootBoxes {
    public class LootboxUIController : MonoBehaviour
    {
        private GameObject panelPrefab;
        [SerializeField] public GridLayoutGroup layoutGroup;
        [SerializeField] public LootBoxUIOpen openUI;
        // Start is called before the first frame update
        public void init(List<LootboxCount> lootBoxes) {
            foreach (LootboxCount lootboxCount in lootBoxes) {
                loadLootbox(lootboxCount);
            }
        }

        private void loadLootbox(LootboxCount lootboxCount) {
            if (panelPrefab == null) {
                panelPrefab = Resources.Load<GameObject>("UI/Lootbox/LootboxPanel");
            }
            /*
            GameObject panel = GameObject.Instantiate(panelPrefab);
            LootBoxPanel lootBoxPanel = panel.GetComponent<LootBoxPanel>();
            lootBoxPanel.set(lootboxCount,openUI);
            panel.transform.SetParent(layoutGroup.transform,false);
            */
        }
    }

}
