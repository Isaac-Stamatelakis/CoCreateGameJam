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
using Items;

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
        [SerializeField] private Button investButton;
        [SerializeField] private Button withDrawButton;
        [SerializeField] private Sprite nullSprite;
        [SerializeField] private ColorableDisplayableList displayableListPrefab;
        [SerializeField] private CurrencyCountPopUp currencyCountPopUpPrefab;
        private TradingCreature TradingCreature {get => TradingController.Instance.TradingCreatures[index];}
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

        private List<GameObject> invisibleWhenCreatureNull;
        private List<GameObject> invisibleWhenCurrencyNull;

        public void Awake() {
            invisibleWhenCreatureNull = new List<GameObject>{
                moodImage.gameObject,
                moodText.gameObject,
                petButton.gameObject,
            };
            invisibleWhenCurrencyNull = new List<GameObject>{
                investButton.gameObject,
                withDrawButton.gameObject
            };
        }

        public void FixedUpdate() {
            refreshDisplay();
        }
        private void selectCreature(int playerCreatureIndex) {
            TradingCreature currentCreature = TradingCreature;
            List<EquipedCreeture> playerCreatures = PlayerIO.Instance.EquipedCreetures;
            bool nullSelected = playerCreatureIndex < 0;
            EquipedCreeture playerCreature = nullSelected ? null : playerCreatures[playerCreatureIndex];
            if (!nullSelected) {
                playerCreatures.RemoveAt(playerCreatureIndex);
            }
            if (currentCreature.EquipedCreeture != null) {
                playerCreatures.Add(currentCreature.EquipedCreeture);
            }
            currentCreature.EquipedCreeture = playerCreature;
        }

        private void selectCurrency(int currencyIndex) {
            TradingCreature currentCreature = TradingCreature;
            List<DoubleItemSlot> currencyCounts = PlayerIO.Instance.Currencies;
            bool nullSelected = currencyIndex < 0;
            DoubleItemSlot currencyCount = nullSelected ? null : currencyCounts[currencyIndex];
            if (currentCreature.Currency != null) {
                DoubleItemSlot creatureCurrencyCount = new DoubleItemSlot(
                    currentCreature.Currency,
                    currentCreature.Amount
                );
                ItemSlotFactory.insertList<DoubleItemSlot>(currencyCounts,creatureCurrencyCount);
            }
            if (currencyCount == null) {
                currentCreature.CreatureCurrency = null;
            } else {
                currentCreature.CreatureCurrency = new DoubleItemSlot(currencyCount.Lootable,0); 
            }
            
        }



        private void selectionButton<T>(string title, List<T> elements, DisplayableClickCallback callback) where T : IDisplayable{
            displayListParameters.title = title;
            ColorableDisplayableList displayableList = GameObject.Instantiate(displayableListPrefab);
            List<IDisplayable> displayables = new List<IDisplayable>();
            foreach (T element in elements) {
                if (element == null) {
                    continue;
                }
                displayables.Add((IDisplayable)element);
            }
            displayableList.display(displayables,callback,displayListParameters);
            Canvas canvas = GlobalUtils.getCanvas(transform);
            displayableList.transform.SetParent(canvas.transform,false);
        }

        private void refreshDisplay() {
            TradingCreature tradingCreature = TradingCreature;
            if (tradingCreature.EquipedCreeture == null) {
                creatureImage.sprite = nullSprite;
                creatureDescription.text = "Click to Select A Creature";
                foreach (GameObject invisibleWhenNull in invisibleWhenCreatureNull) {
                    invisibleWhenNull.gameObject.SetActive(false);
                }
            } else {
                foreach (GameObject invisibleWhenNull in invisibleWhenCreatureNull) {
                    invisibleWhenNull.gameObject.SetActive(true);
                }
                creatureImage.sprite = tradingCreature.EquipedCreeture.getSprite();
                creatureDescription.text = getCreatureDescription(tradingCreature);
            }
            if (tradingCreature.Currency == null) {
                foreach (GameObject invisibleWhenNull in invisibleWhenCurrencyNull) {
                    invisibleWhenNull.gameObject.SetActive(false);
                }
                currencyName.text = "";
                currencyAcroynm.text = "";
                currencyAmount.text = "";
                hourChange.text = "";
                currencyNullText.gameObject.SetActive(true);
            } else {
                foreach (GameObject invisibleWhenNull in invisibleWhenCurrencyNull) {
                    invisibleWhenNull.gameObject.SetActive(true);
                }
                currencyNullText.gameObject.SetActive(false);
                currencyName.text = tradingCreature.Currency.name;
                currencyAcroynm.text = tradingCreature.Currency.Acroynm;
                currencyAmount.text = $"{tradingCreature.Amount:F1}";
                hourChange.text = $"{tradingCreature.Amount:F1}";
            }
        }
        public override void display(TradingCreature element)
        {
            creatureImage.GetComponent<Button>().onClick.RemoveAllListeners();
            creatureImage.GetComponent<Button>().onClick.AddListener(() => {
                selectionButton<EquipedCreeture>(
                    title: "Select a Creature",
                    elements: PlayerIO.Instance.EquipedCreetures,
                    selectCreature
                );
            });

            currencyImage.transform.parent.GetComponent<Button>().onClick.RemoveAllListeners();
            currencyImage.transform.parent.GetComponent<Button>().onClick.AddListener(() => {
                selectionButton<DoubleItemSlot>(
                    title: "Select a Currency",
                    elements: PlayerIO.Instance.Currencies,
                    selectCurrency
                );
            });
            investButton.onClick.RemoveAllListeners();
            withDrawButton.onClick.RemoveAllListeners();
            investButton.onClick.AddListener(() => {
                
                displayCurrencyPopUp(CurrencyPopUpDisplayMode.Invest);
            });

            withDrawButton.onClick.AddListener(() => {
                displayCurrencyPopUp(CurrencyPopUpDisplayMode.Withdraw);
            });


            refreshDisplay();
        }

        private void displayCurrencyPopUp(CurrencyPopUpDisplayMode mode) {
            DoubleItemSlot playerCurrency = ItemSlotFactory.matchList<DoubleItemSlot>(
                PlayerIO.Instance.Currencies,
                new DoubleItemSlot(TradingCreature.Currency,TradingCreature.Amount)
            );
            CurrencyCountPopUp currencyCountPopUp = GameObject.Instantiate(currencyCountPopUpPrefab);
                currencyCountPopUp.display(mode,TradingCreature.CreatureCurrency,playerCurrency);
                Canvas canvas = GlobalUtils.getCanvas(transform);
                currencyCountPopUp.transform.SetParent(canvas.transform,false);
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
            float hoursTraded = element.TradingHistory.TradingMinutes/60f;
            return $"{nickname} has been trading {currencyName} for {hoursTraded:F1} hours";
        }
    }

}
