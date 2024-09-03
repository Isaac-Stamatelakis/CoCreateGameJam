using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI {
    public class SliderTextDisplayUI : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI text;
        public void display(float current, int max) {
            if (max == 0) {
                slider.value = 0;
            } else {
                slider.value = current / max;
            }
            
            //text.text = $"{current}/{max}";
        }
    }

}
