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
        public void display(int current, int max) {
            slider.value = ((float) current) / max;
            text.text = $"{current}/{max}";
        }
    }

}
