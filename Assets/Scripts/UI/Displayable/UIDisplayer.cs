using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Displayables {
    public delegate void DisplayableClickCallback(int index);
    public abstract class UIDisplayer<T> : MonoBehaviour, IPointerClickHandler where T : IDisplayable
    {
        [SerializeField] private Image background;
        private DisplayableClickCallback callback;
        private int index;
        private Transform parentList;
        public void initalize(IDisplayable displayable, DisplayableClickCallback callback, int index, DisplayListParameters displayListParameters, Transform parentList) {
            this.callback = callback;
            this.index = index;
            this.parentList = parentList;
            background.color = displayListParameters.primaryColor;
            display(displayable, displayListParameters);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right) {
                callback(index);
                GameObject.Destroy(parentList.gameObject);
            }
        }
        public abstract void display(IDisplayable displayable, DisplayListParameters displayListParameters);
    }

}
