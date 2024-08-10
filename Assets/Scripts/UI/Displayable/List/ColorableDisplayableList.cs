using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace UI.Displayables {
    public class ColorableDisplayableList : MonoBehaviour
    {
        private readonly float LIST_HEIGHT = 100;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Image mainBackground;
        [SerializeField] private Image scrollViewBackground;
        [SerializeField] private Image titleBackground;
        [SerializeField] private Image scrollViewHandle;
        [SerializeField] private GridLayoutGroup list;
        [SerializeField] private Button backButton;
        [SerializeField] private Button nullButton;
        [SerializeField] private DisplayableSelectorListElement listElementPrefab;
        public void display(List<IDisplayable> displayables, DisplayableClickCallback callback, DisplayListParameters displayListParameters) {
            title.text = displayListParameters.title;
            title.color = displayListParameters.primaryTextColor;

            mainBackground.color = displayListParameters.primaryBackgroundColor;
            scrollViewBackground.color = displayListParameters.secondaryBackgroundColor;
            titleBackground.color = displayListParameters.secondaryBackgroundColor;
            scrollViewHandle.color = displayListParameters.scrollBarColor;
            
            RectTransform rectTransform = (RectTransform) transform;
            Vector2 anchorMin = new Vector2(0.5f,0.5f);
            Vector2 anchorMax = new Vector2(0.5f,0.5f);
            Vector2 sizeDelta = new Vector2(0,0);

            nullButton.onClick.AddListener(() => {
                callback(-1);
                GameObject.Destroy(gameObject);
            });
            nullButton.GetComponent<Image>().color = displayListParameters.secondaryBackgroundColor;
            backButton.onClick.AddListener(() => {
                GameObject.Destroy(gameObject);
            });
            backButton.GetComponent<Image>().color = displayListParameters.secondaryBackgroundColor;
            if (displayListParameters.xSize is AbsoluteListSize absoluteListSizeX) {
                sizeDelta.x = absoluteListSizeX.size;
            } else if (displayListParameters.xSize is AnchorListSize anchorListSizeX) {
                anchorMin.x = anchorListSizeX.min;
                anchorMax.x = anchorListSizeX.max;
            }
            if (displayListParameters.ySize is AbsoluteListSize absoluteListSizeY) {
                sizeDelta.y = absoluteListSizeY.size;
            } else if (displayListParameters.ySize is AnchorListSize anchorListSizeY) {
                anchorMin.y = anchorListSizeY.min;
                anchorMax.y = anchorListSizeY.max;
            }
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.sizeDelta = sizeDelta;

            Rect rect = rectTransform.rect;
            float xSize = rect.size.x;
            list.cellSize = new Vector2(xSize,LIST_HEIGHT);
            
            GlobalUtils.deleteChildren(list.transform);
            for (int i = 0; i < displayables.Count; i++)
            {
                DisplayableSelectorListElement displayableListElement = GameObject.Instantiate(listElementPrefab);
                displayableListElement.initalize(displayables[i],callback,i,displayListParameters,transform);
                displayableListElement.transform.SetParent(list.transform,false);
            }
        }

    }

    public class DisplayListParameters {
        public string title;
        public Color primaryColor;
        public Color primaryBackgroundColor;
        public Color secondaryBackgroundColor;
        public Color primaryTextColor;
        public Color secondaryTextColor;
        public Color scrollBarColor;
        public ListSize xSize;
        public ListSize ySize;

        public DisplayListParameters(string title, Color primaryColor, Color primaryBackgroundColor, Color secondaryBackgroundColor, Color primaryTextColor, Color secondaryTextColor, Color scrollBarColor, ListSize xSize, ListSize ySize)
        {
            this.title = title;
            this.primaryColor = primaryColor;
            this.primaryBackgroundColor = primaryBackgroundColor;
            this.secondaryBackgroundColor = secondaryBackgroundColor;
            this.primaryTextColor = primaryTextColor;
            this.secondaryTextColor = secondaryTextColor;
            this.scrollBarColor = scrollBarColor;
            this.xSize = xSize;
            this.ySize = ySize;
        }
    }

    public interface ListSize {

    }

    public class AbsoluteListSize : ListSize {
        public float size;

        public AbsoluteListSize(float size)
        {
            this.size = size;
        }
    }
    public class AnchorListSize : ListSize {
        public float min;
        public float max;

        public AnchorListSize(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}

