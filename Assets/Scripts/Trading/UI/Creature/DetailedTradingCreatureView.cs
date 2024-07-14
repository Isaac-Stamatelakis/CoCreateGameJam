using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Inventory;
using UnityEngine.UI;
using TMPro;
using System;
using Trading;
using UI.Displayables;
using Creatures;
using System.Linq;
using Player;

namespace Trading.UI {
    public class DetailedTradingCreatureView : UIInventoryDisplayer<TradingCreature>
    {
        [SerializeField] private Image creatureImage;
        [SerializeField] private TextMeshProUGUI creatureDescription;
        [SerializeField] private Image moodImage;
        [SerializeField] private Button petButton;
        [SerializeField] private TextMeshProUGUI moodText;
        [SerializeField] private Image currencyImage;
        [SerializeField] private TextMeshProUGUI currencyAcroynm;
        [SerializeField] private TextMeshProUGUI currencyName;
        [SerializeField] private TextMeshProUGUI currencyAmount;
        [SerializeField] private TextMeshProUGUI hourChange;
        [SerializeField] private TextMeshProUGUI currencyNullText;
        [SerializeField] private Sprite nullSprite;
        [SerializeField] private DisplayableList displayableListPrefab;
        private int index;
        private readonly DisplayListParameters displayListParameters = new DisplayListParameters(
            title: null,
            primaryColor: GlobalUtils.fromHex("#0AA693"),
            primaryBackgroundColor: GlobalUtils.fromHex("#1D2128"),
            secondaryBackgroundColor: GlobalUtils.fromHex("#1A152A"),
            primaryTextColor: Color.white,
            secondaryTextColor: Color.gray,
            scrollBarColor: GlobalUtils.fromHex("#00B346"),
            new AbsoluteListSize(500),
            new AnchorListSize(0.1f,0.9f)
        );
        private void selectCreature(int index) {
            List<TradingCreature> currentCreatures = TradingController.Instance.TradingCreatures;
            TradingCreature tradingCreature = currentCreatures[index];
            EquipedCreeture equipedCreeture = tradingCreature != null ? tradingCreature.EquipedCreeture : null;
            TradingController.Instance.TradingCreatures[this.index].EquipedCreeture = PlayerIO.Instance.EquipedCreetures[index];
            PlayerIO.Instance.EquipedCreetures.RemoveAt(index);
            if (equipedCreeture != null) {
                PlayerIO.Instance.EquipedCreetures.Add(equipedCreeture);
            }
        }

        private void selectCurrency(int index) {
            
        }
        public override void display(TradingCreature element, InventoryUI<TradingCreature> inventory, int index)
        {
            this.index = index;
            List<GameObject> invisibleWhenNullCreatureObjects = new List<GameObject>{
                moodImage.gameObject,
                petButton.gameObject,
                moodText.gameObject
            };
            creatureImage.GetComponent<Button>().onClick.RemoveAllListeners();
            creatureImage.GetComponent<Button>().onClick.AddListener(() => {
                displayListParameters.title = "Select a Creature";
                DisplayableList displayableList = GameObject.Instantiate(displayableListPrefab);
                List<IDisplayable> displayables = PlayerIO.Instance.EquipedCreetures.Cast<IDisplayable>().ToList();
                displayableList.display(displayables,selectCreature,displayListParameters);
                displayableList.transform.SetParent(transform.parent,false);
            });

            currencyImage.transform.parent.GetComponent<Button>().onClick.RemoveAllListeners();
            currencyImage.transform.parent.GetComponent<Button>().onClick.AddListener(() => {
                displayListParameters.title = "Select a Currency";
                DisplayableList displayableList = GameObject.Instantiate(displayableListPrefab);
                List<IDisplayable> displayables = PlayerIO.Instance.EquipedCreetures.Cast<IDisplayable>().ToList();
                displayableList.display(displayables,selectCurrency,displayListParameters);
                displayableList.transform.SetParent(transform.parent,false);
            });

            if (element.EquipedCreeture == null) {
                creatureImage.sprite = nullSprite;
                creatureDescription.text = "Click to Select A Creature";
                foreach (GameObject invisibleWhenNull in invisibleWhenNullCreatureObjects) {
                    invisibleWhenNull.gameObject.SetActive(false);
                }
            } else {
                foreach (GameObject invisibleWhenNull in invisibleWhenNullCreatureObjects) {
                    invisibleWhenNull.gameObject.SetActive(true);
                }
                creatureImage.sprite = element.EquipedCreeture.getSprite();
                creatureDescription.text = getCreatureDescription(element);
            }
            if (element.Currency == null) {
                currencyName.text = "";
                currencyAcroynm.text = "";
                currencyAmount.text = "";
                hourChange.text = "";
                currencyNullText.gameObject.SetActive(true);
            } else {
                currencyNullText.gameObject.SetActive(false);
                currencyName.text = element.Currency.name;
                currencyAcroynm.text = element.Currency.Acroynm;
                currencyAmount.text = $"{element.Amount : F1}";
                hourChange.text = $"{element.Amount : F1}";
            }
        }

        private string getCreatureDescription(TradingCreature element) {
            Currency currency = element.Currency;
            string nickname = element.EquipedCreeture.Nickname;
            if (nickname == null) {
                nickname = element.EquipedCreeture.Creeture.name;
            }
            if (currency == null) {
                return $"{nickname} is currently relaxing";
            }
            string currencyName = currency.name;
            float hoursTraded = element.TradingMinutes/60f;
            return $"{nickname} has been trading {currencyName} for {hoursTraded : F1}";
        }
    }

}
