using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory {
    public class InventoryUI<T> : MonoBehaviour where T : IDisplayable
    {
        [SerializeField] protected UIDisplayer<T> slotPrefab;
        [SerializeField] protected Color highlightColor;
        protected List<T> elements;
        protected List<UIDisplayer<T>> slots;
        private int currentlyHighlightedSlot = -1;
        public void display(List<T> elements) {
            GlobalUtils.deleteChildren(transform);
            slots = new List<UIDisplayer<T>>();
            this.elements = elements;
            loadSlots();
        }
        protected void loadSlots() {
            RectTransform rectTransform = GetComponent<RectTransform>();
            GridLayoutGroup gridLayoutGroup = GetComponent<GridLayoutGroup>();

            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            float width = Mathf.Abs(corners[2].x - corners[0].x);
            float height = Mathf.Abs(corners[2].y - corners[0].y);
            int itemsPerRow = (int) (width/gridLayoutGroup.cellSize.x);
            slots = new List<UIDisplayer<T>>();
            for (int i = 0; i < elements.Count; i++) {
                slots.Add(null);
                loadSlot(i);
            }
        }
        public void highlightSlot(int i) {
            if (i >= slots.Count) {
                Debug.LogWarning($"Tried to highlight slot out of range {i}");
                return;
            }
            if (i == currentlyHighlightedSlot) {
                return;
            }
            if (slots[i] is not IPanelDisplay panelDisplay) {
                Debug.LogWarning("Tried to highlight non panel display slot");
                return;
            }
            
            if (currentlyHighlightedSlot >= 0) {
                Color defaultColor = panelDisplay.getColor();
                ((IPanelDisplay) slots[currentlyHighlightedSlot]).setPanelColor(defaultColor);
            }
            currentlyHighlightedSlot = i;
            panelDisplay.setPanelColor(highlightColor);
        }

        protected void loadSlot(int i) { 
            if (i >= elements.Count) {
                Debug.LogWarning($"Tried to display element at out of range index {i}");
                return;
            }
            UIDisplayer<T> displayer = GameObject.Instantiate(slotPrefab);
            slots[i] = displayer;
            displayer.display(elements[i],this,i);
            displayer.transform.SetParent(transform,false);
            displayer.name = $"slot{i}";
        }

        public void refresh() {
            int i = 0;
            while (i < slots.Count) {
                slots[i].display(elements[i],this,i);
                i++;
            }
            while (i < elements.Count) {
                loadSlot(i);
                i++;
            }
        }
    }
}

