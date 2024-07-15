using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI.Inventory;

namespace Trading.UI {
    public class CreatureTradingUIPage : MonoBehaviour
    {
        [SerializeField] private TradingCreatureList tradingCreatureList;
        [SerializeField] private DetailedTradingCreatureView detailedTradingCreatureView;
        [SerializeField] private AggregateTradingCreatureDisplay aggregateTradingCreatureDisplay;
        [SerializeField] private Button backButton;
        [SerializeField] private Button homeButton;
        public void Start() {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() => {
                GameObject.Destroy(gameObject);
            });

            homeButton.onClick.RemoveAllListeners();
            homeButton.onClick.AddListener(() => {
                detailedTradingCreatureView.gameObject.SetActive(false);
                aggregateTradingCreatureDisplay.gameObject.SetActive(true);
            });
        }
        public void display() {
            tradingCreatureList.reset();
            tradingCreatureList.display(TradingController.Instance.TradingCreatures);
        }

        public void displayDetailedView(int index) {
            aggregateTradingCreatureDisplay.gameObject.SetActive(false);
            detailedTradingCreatureView.gameObject.SetActive(true);
            detailedTradingCreatureView.display(TradingController.Instance.TradingCreatures[index],tradingCreatureList,index);
        }

        public void FixedUpdate() {
            tradingCreatureList.refresh();
        }
    }
}

