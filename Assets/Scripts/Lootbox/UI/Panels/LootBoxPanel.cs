using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UI.Inventory;
using UnityEngine.EventSystems;

namespace LootBoxes {
    public interface IClickableUIElement {
        public void leftClick();
        public void rightClick();
    }
    public abstract class LootBoxPanel : UIDisplayer<LootboxCount>, IClickableUIElement, IPointerClickHandler
    {
        [SerializeField] public Image image;
        [SerializeField] public TextMeshProUGUI text;
        private LootBoxUIOpen open;
        protected LootboxCount lootboxCount;
        protected InventoryUI<LootboxCount> inventory;
        protected int index;

        public override void display(LootboxCount element, InventoryUI<LootboxCount> inventory, int index)
        {
            this.lootboxCount = element;
            image.sprite = lootboxCount.lootBox.getSprite();
            this.inventory = inventory;
            text.text = lootboxCount.count.ToString();
        }
        private void toOpen() {
            open.gameObject.SetActive(true);
            open.load(lootboxCount);
        }

        

        public abstract void leftClick();
        public abstract void rightClick();

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left) {
                leftClick();
            } else if (eventData.button == PointerEventData.InputButton.Right) {
                rightClick();
            }
        }
    }
}

