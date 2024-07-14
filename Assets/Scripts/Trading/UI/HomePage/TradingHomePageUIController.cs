using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trading.UI {
    public enum TradingPage {
        Traders,
        CurrencyExchange,
        FiancialAdvice
    }
    public class TradingHomePageUIController : MonoBehaviour
    {
        [SerializeField] private CreatureTradingUIPage tradingUI;
        [SerializeField] private Transform currencyExchangeUI;
        [SerializeField] private Transform fiancialAdviceUI;
        private Transform currentlyDisplayed;
        public void display(TradingPage page) {
            if (currentlyDisplayed != null) {
                currentlyDisplayed.gameObject.SetActive(false);
            }
            switch (page) {
                case TradingPage.Traders:
                    currentlyDisplayed = tradingUI.transform;
                    tradingUI.display();
                    break;
                case TradingPage.CurrencyExchange:
                    currentlyDisplayed = currencyExchangeUI;
                    break;
                case TradingPage.FiancialAdvice:
                    currentlyDisplayed = fiancialAdviceUI;
                    break;
            }
            currentlyDisplayed.gameObject.SetActive(true);
        }
    }

}
