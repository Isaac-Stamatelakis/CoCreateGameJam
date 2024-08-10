using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Items;

namespace Trading.UI {
    public class CurrencyCountPopUp : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI balance;
        [SerializeField] private Button backButton;
        [SerializeField] private TMP_InputField inputAmount;
        [SerializeField] private GridLayoutGroup quickSelectButtons;
        [SerializeField] private Button submitButton;
        private string lastInput;
        public void display(CurrencyPopUpDisplayMode mode, DoubleItemSlot tradingCreatureCount, DoubleItemSlot playerCount) {
            inputAmount.text = $"{0:F2}";
            lastInput = inputAmount.text;
            double balanceAmount = double.NaN;
            switch (mode) {
                case CurrencyPopUpDisplayMode.Withdraw:
                    balanceAmount = tradingCreatureCount.Amount;
                    break;
                case CurrencyPopUpDisplayMode.Invest:
                    balanceAmount = playerCount.Amount;
                    break;
            }
            balance.text = ItemSlotFactory.formatAmount(balanceAmount);
            
            inputAmount.onValueChanged.AddListener((string input) => {
                if (input.Length == 0) {
                    input = $"{0:F2}";
                }
                double value = ItemSlotFactory.parseAmount(input);
                bool valid = value != double.NaN && value >= 0;
                if (!valid) {
                    inputAmount.text = lastInput;
                } else {
                    if (value > balanceAmount) {
                        value = balanceAmount;
                    }
                    inputAmount.text = $"{value:F2}";
                    lastInput = inputAmount.text;
                }
            });
            switch (mode) {
                case CurrencyPopUpDisplayMode.Withdraw:
                    title.text = "Withdraw";
                    break;
                case CurrencyPopUpDisplayMode.Invest:
                    title.text = "Invest";
                    break;
            }
            List<double> values = ItemSlotFactory.getQuickSelectOptions(balanceAmount);
            GameObject buttonPrefab = quickSelectButtons.transform.GetChild(0).gameObject;
            GlobalUtils.deleteChildren(quickSelectButtons.transform);
            foreach (double value in values) {
                GameObject button = GameObject.Instantiate(buttonPrefab);
                button.transform.SetParent(quickSelectButtons.transform);
                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ItemSlotFactory.formatAmount(value);
                button.GetComponent<Button>().onClick.AddListener(() => {
                    quickSelect(value);
                });
            }

            submitButton.onClick.AddListener(() => {
                double value = ItemSlotFactory.parseAmount(inputAmount.text);
                switch (mode) {
                    case CurrencyPopUpDisplayMode.Withdraw:
                        playerCount.Amount += value;
                        tradingCreatureCount.Amount -= value;
                        break;
                    case CurrencyPopUpDisplayMode.Invest:
                        playerCount.Amount -= value;
                        tradingCreatureCount.Amount += value;
                        break;
                }
                GameObject.Destroy(gameObject);
            });

            backButton.onClick.AddListener(() => {
                GameObject.Destroy(gameObject);
            });

        }
        private void quickSelect(double value) {
            inputAmount.text = ItemSlotFactory.formatAmount(value);
        }
    }

    public enum CurrencyPopUpDisplayMode {
        Withdraw,
        Invest
    }
}

