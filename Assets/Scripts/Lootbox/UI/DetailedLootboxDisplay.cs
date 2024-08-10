using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootBoxes;
using UnityEngine.UI;
using TMPro;
using Items;

namespace UI.Inventory {
    public class DetailedLootboxDisplay : UIInventoryDisplayer<StackableItemSlot>, ILootBoxDisplayer
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI remaining;
        [SerializeField] private Button openButton;
        [SerializeField] private Image image;
        [SerializeField] private LootBoxAnimationController animationControllerPrefab;
        private IntItemSlot itemSlot;
        public override void display(StackableItemSlot element)
        {
            this.itemSlot = (IntItemSlot) element;
            openButton.onClick.AddListener(() => {
                LootBoxAnimationController animationController = GameObject.Instantiate(animationControllerPrefab);
                Canvas canvas = LootBoxAnimationUtils.findCanvas(transform);
                animationController.transform.SetParent(canvas.transform,false);
                StartCoroutine(animationController.open(itemSlot,this));
            });
            rebuild();
        }

        public Vector3 getLootBoxPosition()
        {
            return image.transform.position;
        }

        public void rebuild()
        {
            image.sprite = itemSlot.getSprite();
            inventory.refresh();
        }
    }
}

