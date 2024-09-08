using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

namespace UI.Lists {
    public interface IRefreshableUI {
        public void refresh();
    }
    public abstract class InventoryUI<T> : MonoBehaviour, IRefreshableUI where T : IDisplayable
    {
        [SerializeField] protected UIInventoryDisplayer<T> slotPrefab;
        [SerializeField] protected bool highlightLeftClick = false;
        [SerializeField] protected bool highlightRightClick = false;
        [SerializeField] protected Color highlightColor = Color.yellow;
        protected List<T> elements;
        protected List<UIInventoryDisplayer<T>> slots;
        private int currentlyHighlightedSlot = -1;
        private Color panelColor;
        public bool IsDisplaying {get => slots != null;}
        public void display(List<T> elements) {
            GlobalUtils.deleteChildren(transform);
            slots = new List<UIInventoryDisplayer<T>>();
            this.elements = elements;
            loadSlots();
        }

        public void display(List<T> elements, ColorTheme colorTheme) {
            display(elements);
            panelColor = slotPrefab.GetComponent<Image>().color;
            setTheme(colorTheme);
        }

        public void setTheme(ColorTheme colorTheme) {
            foreach (UIInventoryDisplayer<T> slot in slots) {
                slot.setTheme(colorTheme);
            }
            panelColor = colorTheme.Primary;
        }
        protected void loadSlots() {
            RectTransform rectTransform = GetComponent<RectTransform>();
            GridLayoutGroup gridLayoutGroup = GetComponent<GridLayoutGroup>();
            //Debug.Log(width);
            /*
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            for (int i = 0; i < 4; i++) {
                Debug.Log($"{i} : {corners[i]}");
            }
            */
            
            //float width = Vector3.Distance(corners[0],corners[3]);
            //int itemsPerRow = (int) (rectTransform.rect.width/gridLayoutGroup.cellSize.x);
            //Debug.Log($"{name} {width} {itemsPerRow}");
            //Vector2 size = RectTransformUtility.PixelAdjustRect(rectTransform,rectTransform.GetComponentInParent<Canvas>()).size;
            //Debug.Log($"{size}");
            slots = new List<UIInventoryDisplayer<T>>();
            for (int i = 0; i < elements.Count; i++) {
                addSlot();
            }
            
        }
        protected void highlightSlot(int i) {
            if (i >= slots.Count) {
                Debug.LogWarning($"Tried to highlight slot out of range {i}");
                return;
            }
            bool clickedHighlightedSlot = i == currentlyHighlightedSlot;
            if (clickedHighlightedSlot) {
                return;
            }
            resetHighlight();
            currentlyHighlightedSlot = i;
            setSlotColor(slots[currentlyHighlightedSlot],highlightColor);
        }

        public void resetHighlight() {
            bool slotHighlighted = currentlyHighlightedSlot != -1;
            if (!slotHighlighted) {
                return;
            }
            setSlotColor(slots[currentlyHighlightedSlot],panelColor);
            currentlyHighlightedSlot = -1;
        }


        protected void setSlotColor(UIInventoryDisplayer<T> slot, Color color) {
            if (slot == null) {
                return;
            }
            Image panel = slot.gameObject.GetComponent<Image>();
            if (panel == null) {
                return;
            }
            if (color.a==0) {
                color.a=1;
            }
            panel.color = color;
        }

        protected void addSlot() { 
            int i = slots.Count;
            if (slots.Count >= elements.Count) {
                Debug.LogWarning($"Tried to display element at out of range index {i}");
                return;
            }
            UIInventoryDisplayer<T> displayer = GameObject.Instantiate(slotPrefab);
            slots.Add(displayer);
            displayer.display(elements[i],i,this);
            displayer.transform.SetParent(transform,false);
            displayer.name = $"slot{i}";
        }

        public void leftClickInventory(int index) {
            if (highlightLeftClick) {
                highlightSlot(index);
            }
            leftClick(index);
        }
        public void rightClickInventory(int index) {
            if (highlightRightClick) {
                highlightSlot(index);
            }
            rightClick(index);
        }
        public abstract void rightClick(int index);
        public abstract void leftClick(int index);
        public void refresh() {
            while (slots.Count > elements.Count) {
                GameObject.Destroy(slots.Last());
                slots.RemoveAt(slots.Count-1);
            }
            for (int i = 0; i < slots.Count; i++) {
                slots[i].display(elements[i],i,this);
            }
            int safe = 0;
            while (slots.Count < elements.Count) {
                addSlot();
                safe ++;
                if (safe > 100) {
                    throw new System.Exception("Passed while safe limit");
                }
            }
        }
        public void reset() {
            GlobalUtils.deleteChildren(transform);
            slots = new List<UIInventoryDisplayer<T>>();
        }
    }
}

