using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI.Inventory;

namespace LootBoxes {
    public class LootBoxChainAnimation : LootBoxAnimation
    {
        [SerializeField] public Image chain1;
        [SerializeField] public Image chain2;
        [SerializeField] public Image chainLock;
        [SerializeField] public Image lootboxImage;
        [SerializeField] public Image panel;
        public override IEnumerator execute() {
            yield return StartCoroutine(removeChain(chain1));
            yield return StartCoroutine(removeChain(chain2));
            yield return StartCoroutine(removeChain(chainLock));
        }

        
        

        private IEnumerator removeChain(Image chain) {
            RectTransform chainRect = chain.GetComponent<RectTransform>();
            Vector3 originalPos = chainRect.position;
            float iterations = 64;
            for (float i = 0; i < iterations; i+= 256f/iterations) {
                chainRect.transform.position = originalPos + new Vector3(Random.Range(-10f,10f),Random.Range(-10f,10f),0f);
                chain.color = new Color(1f,1f,1f,1f-i/255f);
                yield return new WaitForFixedUpdate();
            }
            chain.color = new Color(1f,1f,1f,0f);
        }
    }
}

