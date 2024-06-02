using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Levels.Combat {
    public class GameWinUIController : MonoBehaviour
    {
        [SerializeField] public Button continueButton;
        [SerializeField] public Image continueButtonImage;
        public void Start() {
            continueButtonImage.color = new Color(1f,1f,1f,0f);
            StartCoroutine(slowShowButton());
            continueButton.onClick.AddListener(returnToMenu);
        }
        public IEnumerator slowShowButton() {
            for (float i = 0; i <= 255 ;i++) {
                continueButtonImage.color = new Color(1f,1f,1f,i/255f);
                yield return new WaitForEndOfFrame();
            }
            yield return null;
        }
        public void OnDestroy() {
            continueButton.onClick.RemoveAllListeners();
        }
        private void returnToMenu() {
            SceneManager.LoadScene("SelectorScene");
        }
    }

}
