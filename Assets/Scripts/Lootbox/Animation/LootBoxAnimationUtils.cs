using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LootBoxes {
    public static class LootBoxAnimationUtils
    {
        public static IEnumerator blackScreen(Transform transform) {
            GameObject panelObject = new GameObject();
            panelObject.name = "BlackScreen";
            Image panel = panelObject.AddComponent<Image>();
            RectTransform rectTransform = panelObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.position = Vector3.zero;
            panelObject.transform.SetParent(transform,false);
            panelObject.transform.SetAsFirstSibling();
            for (float i = 0; i < 255; i+=16) { //16/25 = 0.64 seconds
                panel.color = new Color(0f,0f,0f,i/255f);
                yield return new WaitForFixedUpdate();
            }
            panel.color = new Color(0f,0f,0f,1f);
        }

        public static Canvas findCanvas(Transform element) {
            Transform parent = element;
            while (parent != null)
            {
                Canvas canvas = parent.GetComponent<Canvas>();
                if (canvas != null)
                {
                    return canvas;
                }
                parent = parent.parent;
            }
            return null;
        }

        public static IEnumerator moveLootbox(Transform transform, Vector2 targetPosition, int duration, float stepDelay) {
            Vector3 currentPosition = transform.position;
            Vector3 distance = (Vector3)targetPosition-currentPosition;
            Vector3 speedVector = distance/duration;
            for (int i = 0; i < duration; i++) { 
                transform.position += speedVector;
                yield return new WaitForSeconds(stepDelay);
            }
        }
    }
}

