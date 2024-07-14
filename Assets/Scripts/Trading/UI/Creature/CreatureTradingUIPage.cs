using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Inventory;

namespace Trading.UI {
    public class CreatureTradingUIPage : MonoBehaviour
    {
        [SerializeField] private TradingCreatureList tradingCreatureList;
        [SerializeField] private DetailedTradingCreatureView detailedTradingCreatureView;
        [SerializeField] private AggregateTradingCreatureDisplay aggregateTradingCreatureDisplay;
        public void display() {
            tradingCreatureList.reset();
            tradingCreatureList.display(TradingController.Instance.TradingCreatures);
        }

        public void displayDetailedView(int index) {
            aggregateTradingCreatureDisplay.gameObject.SetActive(false);
            detailedTradingCreatureView.gameObject.SetActive(true);
            detailedTradingCreatureView.display(TradingController.Instance.TradingCreatures[index],tradingCreatureList,index);
        }
    }
}

