using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UI.Inventory;
using UnityEngine.EventSystems;

namespace Trading.UI {
    public class TradingCreatureListElement : ClickableUIDisplayer<TradingCreature>, IPanelDisplay
    {
        [SerializeField] private Image creatureImage;
        [SerializeField] private Image currencyImage;
        [SerializeField] private TextMeshProUGUI currencyAcroynm;
        [SerializeField] private TextMeshProUGUI currencyName;
        [SerializeField] private TextMeshProUGUI amount;
        [SerializeField] private TextMeshProUGUI change;
        [SerializeField] private TextMeshProUGUI empty;
        [SerializeField] private Transform activeElements;
        [SerializeField] private Image panel;
        private int index;
        private InventoryUI<TradingCreature> inventoryUI;
        public override void display(TradingCreature element, InventoryUI<TradingCreature> inventory, int index)
        {
            this.inventoryUI = inventory;
            this.index = index;
            if (element == null || (element.EquipedCreeture == null && element.Currency == null)) {
                empty.gameObject.SetActive(true);
                activeElements.gameObject.SetActive(false);
            } else {
                empty.gameObject.SetActive(false);
                activeElements.gameObject.SetActive(true);
                if (element.EquipedCreeture == null) {
                    // Todo replace this with different image
                    creatureImage.sprite = null;
                } else {
                    creatureImage.sprite = element.EquipedCreeture.getSprite();
                }
                if (element.Currency == null) {
                    // Todo replace this with different image
                    currencyImage.sprite = null;
                    currencyName.text = "";
                    currencyAcroynm.text = "";
                    amount.text = "";
                    change.text = "";
                } else {
                    currencyImage.sprite = element.Currency.getSprite();
                    currencyName.text = element.Currency.name;
                    currencyAcroynm.text = element.Currency.Acroynm;
                }
            }
            
        }

        public Color getColor()
        {
            return panel.color;
        }

        public override void leftClick()
        {  
            ((TradingCreatureList) inventoryUI).CreatureTradingUIPage.displayDetailedView(index);
        }

        public override void rightClick()
        {
            // Does nothing
        }

        public void setPanelColor(Color color)
        {
            panel.color = color;
        }
    }

}
