using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Inventory;
using Player;
using UnityEngine.UI;
using TMPro;

namespace LootBoxes {
    public class LootBoxAnimationController : MonoBehaviour
    {
        private LootboxCount lootboxCount;
        private ILootBoxDisplayer displayer;
        [SerializeField] private LootableInventoryUI lootableInventoryUI;
        [SerializeField] private Button continueButton;
        public void Start() {
            continueButton.onClick.AddListener(() => {
                GameObject.Destroy(gameObject);
                displayer.rebuild();
            });
            continueButton.gameObject.SetActive(false);
        }
        public IEnumerator open(LootboxCount lootboxCount, ILootBoxDisplayer displayer) {
            
            this.lootboxCount = lootboxCount;
            this.displayer = displayer;
            lootboxCount.count--;
            GameObject animationObject = null;
            LootBox lootBox = lootboxCount.lootBox;
            if (lootBox is PrefabAnimatedLootbox prefabAnimatedLootbox) {
                LootBoxAnimation animation = GameObject.Instantiate(prefabAnimatedLootbox.Animation);
                animationObject = animation.gameObject;
            } else if (lootBox is StandardAnimatedLootBox standardAnimatedLootBox) {
                animationObject = new GameObject();
                animationObject.name = $"{lootBox.name} Animation";
                animationObject.AddComponent<SpriteRenderer>();
                Animator animator = animationObject.AddComponent<Animator>();
                animator.runtimeAnimatorController = standardAnimatedLootBox.AnimatorController;
                animator.StopPlayback();
            }
            Canvas canvas = transform.parent.GetComponent<Canvas>();
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            Vector2 canvasCenter = new Vector2(canvasRect.rect.width/2,canvasRect.rect.height/2);
            animationObject.transform.SetParent(transform,false);
            animationObject.transform.position = displayer.getLootBoxPosition();
            Coroutine a = StartCoroutine(LootBoxAnimationUtils.blackScreen(transform));
            Coroutine b = StartCoroutine(LootBoxAnimationUtils.moveLootbox(animationObject.transform,canvasCenter,25,0.01f));
            yield return a;
            yield return b;
            
            if (lootBox is PrefabAnimatedLootbox) {
                LootBoxAnimation[] lootBoxAnimations = animationObject.GetComponents<LootBoxAnimation>();
                yield return StartCoroutine(lootBoxAnimations[0].execute());
            } else if (lootBox is StandardAnimatedLootBox) {
                Animator animator = animationObject.GetComponent<Animator>();
                animator.StartPlayback();
                yield return waitForAnimation(animator);
            }
            Lootable loot = lootboxCount.lootBox.open();
            PlayerIO.Instance.give(loot);
            lootableInventoryUI.display(new List<Lootable>{loot});
            continueButton.gameObject.SetActive(true);
        }

        private IEnumerator waitForAnimation(Animator animator) {
            int i = 0;
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 && i < 1000)
            {
                i++;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}

