using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootBoxes;
using UnityEngine.UI;
using TMPro;

namespace UI.Inventory {
    public class DetailedLootboxDisplay : UIDisplayer<LootboxCount>, ILootBoxDisplayer
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI remaining;
        [SerializeField] private Button openButton;
        [SerializeField] private Image image;
        [SerializeField] private LootBoxAnimationController animationControllerPrefab;
        private InventoryUI<LootboxCount> inventory;
        private int index;
        private LootboxCount lootboxCount;
        public override void display(LootboxCount element, InventoryUI<LootboxCount> inventory, int index)
        {
            this.inventory = inventory;
            this.lootboxCount = element;
            openButton.onClick.AddListener(() => {
                LootBoxAnimationController animationController = GameObject.Instantiate(animationControllerPrefab);
                Canvas canvas = LootBoxAnimationUtils.findCanvas(transform);
                animationController.transform.SetParent(canvas.transform,false);
                StartCoroutine(animationController.open(lootboxCount,this));
            });
            this.index = index;
            rebuild();
        }

        public Vector3 getLootBoxPosition()
        {
            return image.transform.position;
        }

        public void rebuild()
        {
            image.sprite = lootboxCount.getSprite();
            inventory.refresh();
        }
    }
}

