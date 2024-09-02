using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crafting.Recipes;
using Items;
using UI.Lists;
using System.Linq;
using UnityEngine.UI;

namespace Crafting.UI {
    public interface IRecipeDisplayer {
        public void craft();
        public void setButton(Button button);
    }
    public abstract class RecipeDisplayer<T> : UIInventoryDisplayer<T>, IRecipeDisplayer where T : Recipe
    {
        private Button button;
        public abstract void craft();
        private bool inAnimation = false;
        private Color buttonColor;

        public void setButton(Button button)
        {
            this.button = button;
            buttonColor = button.GetComponent<Image>().color;
        }

        protected void craftFail() {
            if (inAnimation) {
                return;
            }
            StartCoroutine(craftAnimation(5,true,Color.red,20,0));
        }

        protected void craftSucceed() {
            StartCoroutine(craftAnimation(0,false,Color.green,20,0.2f));
        }

        private IEnumerator craftAnimation(float shakeRange, bool lockAnimation, Color color, int fixedUpdates, float initalDelay) {
            if (lockAnimation) {
                inAnimation = true;
            }
            Image image = button.GetComponent<Image>();
            Vector3 originalPosition = button.transform.position;
            image.color = color;
            if (initalDelay > 0) {
                yield return new WaitForSeconds(0.2f);
            }
            for (int i = 0; i < fixedUpdates; i++) {
                float t = (float) i / fixedUpdates;
                image.color = Color.Lerp(color, buttonColor, t);
                if (shakeRange > 0) {
                    button.transform.position = originalPosition + new Vector3(Random.Range(-shakeRange,shakeRange),Random.Range(-shakeRange,shakeRange));
                }
                yield return new WaitForFixedUpdate();
            }
            button.GetComponent<Image>().color = buttonColor;
            button.transform.position = originalPosition;
            inAnimation = false;
        }
    }

}
