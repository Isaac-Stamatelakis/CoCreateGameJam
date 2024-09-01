using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions.Objects {
    public class StandardProjectile : MonoBehaviour, IActionSpawnObject
    {
        [SerializeField] private float speed;
        [SerializeField] private RuntimeAnimatorController hitAnimation;
        [SerializeField] private Animator animator;

        public IEnumerator moveToTarget(Transform target)
        {
            Vector3 toMovePosition = transform.position;
            (Vector3,int) tuple = GlobalUtils.speedAndIterationsToMove(target.position,toMovePosition,speed);
            Vector3 distance = target.position-toMovePosition;
            float angle = Mathf.Atan2(distance.y,distance.x);
            float angleDegrees = Mathf.Rad2Deg*angle;
            transform.rotation = Quaternion.Euler(0, 0, angleDegrees);
            Vector3 movementVector = tuple.Item1;
            int iterations = tuple.Item2;
            while (iterations > 0) {
                if (gameObject == null) {
                    yield break;
                }
                iterations--;
                transform.position += movementVector;
                yield return new WaitForFixedUpdate();
            }
            if (hitAnimation != null) {
                animator.runtimeAnimatorController = hitAnimation;
                while (
                    animator.GetCurrentAnimatorStateInfo(0).IsName(hitAnimation.name) 
                    && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    yield return new WaitForFixedUpdate();
                }
            }
            

        }
    }

}
