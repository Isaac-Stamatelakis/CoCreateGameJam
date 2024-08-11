using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crafting.Recipes;
using Items;
using UI.Inventory;
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

        public void setButton(Button button)
        {
            this.button = button;
        }

        protected void craftFail() {
            if (inAnimation) {
                return;
            }
            StartCoroutine(craftFailAnimation());
        }

        protected void craftSucceed() {
            StartCoroutine(craftSucceedAnimation());
        }

        private IEnumerator craftFailAnimation() {
            inAnimation = true;
            Vector3 originalPosition = button.transform.position;
            Image image = button.GetComponent<Image>();
            Color originalColor = image.color;
            image.color = Color.red;
            float shakeRange = 5;
            for (int i = 0; i < 20; i++) {
                button.transform.position = originalPosition + new Vector3(Random.Range(-shakeRange,shakeRange),Random.Range(-shakeRange,shakeRange));
                yield return new WaitForFixedUpdate();
            }
            button.transform.position = originalPosition;
            button.GetComponent<Image>().color = originalColor;
            inAnimation = false;
        }

        private IEnumerator craftSucceedAnimation() {
            Image image = button.GetComponent<Image>();
            Color originalColor = image.color;
            image.color = Color.green;
            yield return new WaitForSeconds(0.2f);
            button.GetComponent<Image>().color = originalColor;
        }
    }

}
