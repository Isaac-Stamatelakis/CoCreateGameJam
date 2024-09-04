using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public delegate void ToggleCallBack(bool active);
    public class ImageToggleButtonUI : MonoBehaviour
    {
        [SerializeField] private Image panel;
        [SerializeField] private Button button;
        [SerializeField] private Color activeColor = Color.green;
        [SerializeField] private Color inActiveColor = Color.gray;
        [SerializeField] private bool active = true;
        private ToggleCallBack callBack;
        public void setCallBack(ToggleCallBack callBack) {
            this.callBack = callBack;
        }
        public void Start() {
            setColor();
            button.onClick.AddListener(() => {
                toggle();
            });
        }
        public void toggle() {
            setActive(!active);
        }

        public void setActive(bool newState) {
            this.active = newState;
            if (callBack != null) {
                callBack(active);
            }
            setColor();
        }
        public void setActiveNoCallBack(bool newState) {
            this.active = newState;
            setColor();
        }

        private void setColor() {
            panel.color = active ? activeColor : inActiveColor;
        } 
    }
}

