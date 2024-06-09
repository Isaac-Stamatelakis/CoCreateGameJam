using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Player;

namespace LootBoxes {
    public interface INavigatable {
        public void navigate();
    }
    public class LootBoxUIOpen : MonoBehaviour, INavigatable
    {
        [SerializeField] public Button openButton;
        [SerializeField] public Image lootboxImage;
        [SerializeField] public PlayerIO playerIO;
        [SerializeField] public TextMeshProUGUI countText;
        private GameObject lootboxOpenAnimationPrefab;

        private LootboxCount lootboxCount;
        
        public void load(LootboxCount lootboxCount) {
            lootboxImage.gameObject.SetActive(true);
            this.lootboxCount = lootboxCount;
            this.lootboxImage.sprite = lootboxCount.lootBox.sprite;
            this.countText.text = lootboxCount.count.ToString();
            openButton.onClick.AddListener(open);
        }

        private void open() {
            /*
            if (lootboxOpenAnimationPrefab == null) {
                lootboxOpenAnimationPrefab = Resources.Load<GameObject>("UI/Lootbox/LootboxOpenAnimation");
            }
            lootboxCount.count--;
            lootboxImage.gameObject.SetActive(false);
            GameObject instantiated = GameObject.Instantiate(lootboxOpenAnimationPrefab);
            instantiated.transform.SetParent(transform.parent,false);
            LootBoxChainAnimation lootboxOpenAnimation = instantiated.GetComponent<LootBoxChainAnimation>();
            lootboxOpenAnimation.open(lootboxCount.lootBox,this);
            */
        }

        public void navigate()
        {
            gameObject.SetActive(true);
            lootboxImage.gameObject.SetActive(true);
        }
    }

}
