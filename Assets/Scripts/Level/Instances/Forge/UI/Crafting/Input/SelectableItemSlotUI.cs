using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

namespace Crafting.UI {
    public class SelectableItemSlotUI : ItemSlotUI, IPointerClickHandler
    {
        public delegate void IndexClickCallback(int index);
        public delegate void IndexClickClearCallback(int index);
        private IndexClickCallback clickCallback;
        private IndexClickClearCallback clickClearCallback;
        private int index;
        public void sync(IndexClickCallback clickCallback, IndexClickClearCallback clearCallback, int index) {
            this.clickCallback = clickCallback;
            this.clickClearCallback = clearCallback;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left) {
                clickCallback(index);
            } else if (eventData.button == PointerEventData.InputButton.Right) {
                clickClearCallback(index);
            }
        }
    }
}

