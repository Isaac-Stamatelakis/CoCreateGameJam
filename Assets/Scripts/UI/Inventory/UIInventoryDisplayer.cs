using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Lists {
    public abstract class UIInventoryDisplayer<T> : MonoBehaviour, IUIDisplayer<T>, IPointerClickHandler where T : IDisplayable
    {
        protected InventoryUI<T> inventory;
        protected int index;
        public abstract void display(T element);
        public void display(T element, int index, InventoryUI<T> inventory) {
            this.index = index;
            this.inventory = inventory;
            display(element);
        }

        public void setTheme(ColorTheme colorTheme) {
            Image panel = GetComponent<Image>();
            if (panel != null) {
                if (colorTheme == null) {
                    panel.enabled = false;
                } else {
                    panel.color = colorTheme.Primary;
                }
            }
            Outline outline = GetComponent<Outline>();
            if (outline != null) {
                if (colorTheme == null) {
                    outline.enabled = false;
                } else {
                    outline.effectColor = colorTheme.Tertiary;
                }
                
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (inventory == null) {
                return;
            }
            if (eventData.button == PointerEventData.InputButton.Left) {
                inventory.leftClickInventory(index);
            } else if (eventData.button == PointerEventData.InputButton.Right) {
                inventory.rightClickInventory(index);
            }
        }
    }
}

