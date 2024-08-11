using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory {
    public abstract class InventoryUI<T> : MonoBehaviour where T : IDisplayable
    {
        [SerializeField] protected UIInventoryDisplayer<T> slotPrefab;
        [SerializeField] protected Color highlightColor = Color.yellow;
        protected List<T> elements;
        protected List<UIInventoryDisplayer<T>> slots;
        private int currentlyHighlightedSlot = -1;
        public bool IsDisplaying {get => slots != null;}
        public void display(List<T> elements) {
            GlobalUtils.deleteChildren(transform);
            slots = new List<UIInventoryDisplayer<T>>();
            this.elements = elements;
            loadSlots();
        }

        public void display(List<T> elements, ColorTheme colorTheme) {
            display(elements);
            setTheme(colorTheme);
        }

        public void setTheme(ColorTheme colorTheme) {
            foreach (UIInventoryDisplayer<T> slot in slots) {
                slot.setTheme(colorTheme);
            }
        }
        protected void loadSlots() {
            RectTransform rectTransform = GetComponent<RectTransform>();
            GridLayoutGroup gridLayoutGroup = GetComponent<GridLayoutGroup>();
            /*
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            float width = Mathf.Abs(corners[2].x - corners[0].x);
            float height = Mathf.Abs(corners[2].y - corners[0].y);
            int itemsPerRow = (int) (width/gridLayoutGroup.cellSize.x);
            */
            slots = new List<UIInventoryDisplayer<T>>();
            for (int i = 0; i < elements.Count; i++) {
                slots.Add(null);
                loadSlot(i);
            }
        }
        protected void highlightSlot(int i) {
            if (i >= slots.Count) {
                Debug.LogWarning($"Tried to highlight slot out of range {i}");
                return;
            }
            if (i == currentlyHighlightedSlot) {
                return;
            }
            Image panel = slotPrefab.GetComponent<Image>();
            if (panel == null) {
                return;
            }
            if (currentlyHighlightedSlot > 0) {
                setSlotColor(slots[currentlyHighlightedSlot],panel.color); // Resets color of currently highlighted
            }
            currentlyHighlightedSlot = i;
            setSlotColor(slots[currentlyHighlightedSlot],highlightColor);
        }

        protected void setSlotColor(UIInventoryDisplayer<T> slot, Color color) {
            if (slot == null) {
                return;
            }
            Image panel = slot.gameObject.GetComponent<Image>();
            if (panel == null) {
                return;
            }
            panel.color = slotPrefab.GetComponent<Image>().color;
        }

        protected void loadSlot(int i) { 
            if (i >= elements.Count) {
                Debug.LogWarning($"Tried to display element at out of range index {i}");
                return;
            }
            UIInventoryDisplayer<T> displayer = GameObject.Instantiate(slotPrefab);
            slots[i] = displayer;
            displayer.display(elements[i],i,this);
            displayer.transform.SetParent(transform,false);
            displayer.name = $"slot{i}";
        }

        public abstract void rightClick(int index);
        public abstract void leftClick(int index);

        public void refresh() {
            int i = 0;
            while (i < slots.Count) {
                slots[i].display(elements[i],i,this);
                i++;
            }
            while (i < elements.Count) {
                loadSlot(i);
                i++;
            }
        }
        public void reset() {
            GlobalUtils.deleteChildren(transform);
            slots = new List<UIInventoryDisplayer<T>>();
        }
    }
}

