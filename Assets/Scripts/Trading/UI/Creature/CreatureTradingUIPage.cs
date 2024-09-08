using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI.Lists;

namespace Trading.UI {
    public class CreatureTradingUIPage : MonoBehaviour, IDisplayController
    {
        [SerializeField] private TradingCreatureList tradingCreatureList;
        [SerializeField] private Transform detailedViewContainer;
        [SerializeField] private AggregateTradingCreatureDisplay aggregateTradingCreatureDisplay;
        [SerializeField] private Button backButton;
        [SerializeField] private Button homeButton;
        public void Start() {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() => {
                gameObject.SetActive(false);
            });

            homeButton.onClick.RemoveAllListeners();
            homeButton.onClick.AddListener(() => {
                GlobalUtils.deleteChildren(detailedViewContainer);
                tradingCreatureList.resetHighlight();
                aggregateTradingCreatureDisplay.gameObject.SetActive(true);
            });
        }
        public void display() {
            tradingCreatureList.reset();
            tradingCreatureList.display(TradingController.Instance.TradingCreatures);
        }
        public void FixedUpdate() {
            tradingCreatureList.refresh();
        }

        public Transform getDetailedDisplayContainer()
        {
            return detailedViewContainer;
        }
    }
}

