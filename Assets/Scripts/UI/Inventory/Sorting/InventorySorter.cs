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


        protected abstract void applySorting();


        
    }
}

