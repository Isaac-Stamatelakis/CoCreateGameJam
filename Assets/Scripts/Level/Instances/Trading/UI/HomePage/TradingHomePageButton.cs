using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Trading.UI {
    public class TradingHomePageButton : MonoBehaviour
    {
        [SerializeField] private TradingPage page;
        public void Start() {
            Transform parentTransform = transform.parent;
            while (parentTransform != null) {
                TradingHomePageUIController controller = parentTransform.GetComponent<TradingHomePageUIController>();
                if (controller != null) {
                    GetComponent<Button>().onClick.AddListener(() => {
                        controller.display(page);
                    });
                    break;
                }
                parentTransform = parentTransform.parent;
            }
        }
    }
}

