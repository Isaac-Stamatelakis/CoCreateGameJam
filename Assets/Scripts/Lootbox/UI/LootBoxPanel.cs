using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LootBoxModule {
    public class LootBoxPanel : MonoBehaviour
    {
        [SerializeField] public Image image;
        [SerializeField] public Button button;
        [SerializeField] public TextMeshProUGUI text;
        public void set(LootboxCount lootboxCount) {
            image.sprite = lootboxCount.lootBox.sprite;
            text.text = lootboxCount.count.ToString();
            button.onClick.AddListener(toOpen);
        }
        private void toOpen() {
            Debug.Log("HI");
        }
    }
}

