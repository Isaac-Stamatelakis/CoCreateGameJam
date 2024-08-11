using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Levels.Combat {
    public class DamageIndicatorUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI amountText;
        public void display(float damage, DamageType damageType, Vector3 targetPosition) {
            amountText.text = $"{damage:F0}";
            RectTransform rectTransform = (RectTransform) transform;
            float RANDOM_RANGE = 32;
            Vector3 randomOffset = new Vector3(Random.Range(-RANDOM_RANGE,RANDOM_RANGE),Random.Range(-RANDOM_RANGE,RANDOM_RANGE));
            rectTransform.position = targetPosition+randomOffset;
            StartCoroutine(move());
        }

        private IEnumerator move() {
            float speed = 0.25f;
            int ITERATIONS = 50;
            for (int i = 0; i < ITERATIONS; i++) {
                Vector3 position = transform.position;
                position.y += speed;
                transform.position = position;
                yield return new WaitForFixedUpdate();
            }
            GameObject.Destroy(gameObject);
        }
    }
}

