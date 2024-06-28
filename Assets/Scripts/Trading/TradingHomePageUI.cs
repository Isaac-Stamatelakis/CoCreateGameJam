using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Trading {
    public class TradingHomePageUI : MonoBehaviour
    {
        [SerializeField] private Button button;
        
        public void Start() {
            button.onClick.AddListener(() => {

            });
        }
    }

}
