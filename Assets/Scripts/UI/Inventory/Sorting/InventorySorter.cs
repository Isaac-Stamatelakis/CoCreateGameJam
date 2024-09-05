using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI.Lists {
    public abstract class InventorySorter<T> : MonoBehaviour where T : IDisplayable
    {
        [SerializeField] private InventoryUI<T> inventoryUI;
        [SerializeField] private TMP_InputField searchBar;
        private string searchValue;
        protected List<T> displayed;
        protected List<T> elements;
        private int previousSearchLength;
        public void initalize(List<T> elements) {
            this.elements = elements;
            addSortingFeatures();
            display();
            if (searchBar != null) {
                searchBar.onValueChanged.AddListener((string value) => {
                    this.searchValue = value.ToLower();
                    display();
                });
            }
            
        }

        protected abstract void addSortingFeatures();

        protected void display() {
            displayed = InventorySortingUtils.sortSearch(searchValue, elements, displayed,previousSearchLength);
            applySorting();
            inventoryUI.display(displayed);
        }

        public void refresh() {
            displayed = elements;
            display();
        }


        protected abstract void applySorting();
        public T getSelectedElement(int index) {
            if (index < 0 || index >= displayed.Count) {
                return default(T);
            }
            return displayed[index];
        }

        public void removeSelectedElement(int index) {
            T element = getSelectedElement(index);
            if (element == null) {
                return;
            }
            int indexInOriginal = elements.IndexOf(element);
            elements.RemoveAt(index);
            refresh();
        }

        public void insert(T element) {
            elements.Add(element);
            refresh();
        }


        
    }
}

