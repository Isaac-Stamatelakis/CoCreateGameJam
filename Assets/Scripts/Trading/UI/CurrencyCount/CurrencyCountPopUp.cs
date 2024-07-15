using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
        public void display(CurrencyPopUpDisplayMode mode, CurrencyCount tradingCreatureCount, CurrencyCount playerCount) {
            inputAmount.text = $"{0:F2}";
            lastInput = inputAmount.text;
            double balanceAmount = double.NaN;
            switch (mode) {
                case CurrencyPopUpDisplayMode.Withdraw:
                    balanceAmount = tradingCreatureCount.amount;
                    break;
                case CurrencyPopUpDisplayMode.Invest:
                    balanceAmount = playerCount.amount;
                    break;
            }
            balance.text = CurrencyCountUtils.format(balanceAmount);
            
            inputAmount.onValueChanged.AddListener((string input) => {
                if (input.Length == 0) {
                    input = $"{0:F2}";
                }
                double value = CurrencyCountUtils.parse(input);
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
            List<double> values = CurrencyCountUtils.getQuickSelectOptions(balanceAmount);
            GameObject buttonPrefab = quickSelectButtons.transform.GetChild(0).gameObject;
            GlobalUtils.deleteChildren(quickSelectButtons.transform);
            foreach (double value in values) {
                GameObject button = GameObject.Instantiate(buttonPrefab);
                button.transform.SetParent(quickSelectButtons.transform);
                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = CurrencyCountUtils.format(value);
                button.GetComponent<Button>().onClick.AddListener(() => {
                    quickSelect(value);
                });
            }

            submitButton.onClick.AddListener(() => {
                double value = CurrencyCountUtils.parse(inputAmount.text);
                switch (mode) {
                    case CurrencyPopUpDisplayMode.Withdraw:
                        playerCount.amount += value;
                        tradingCreatureCount.amount -= value;
                        break;
                    case CurrencyPopUpDisplayMode.Invest:
                        playerCount.amount -= value;
                        tradingCreatureCount.amount += value;
                        break;
                }
                GameObject.Destroy(gameObject);
            });

            backButton.onClick.AddListener(() => {
                GameObject.Destroy(gameObject);
            });

        }
        private void quickSelect(double value) {
            inputAmount.text = CurrencyCountUtils.format(value);
        }
    }

    public enum CurrencyPopUpDisplayMode {
        Withdraw,
        Invest
    }
}

