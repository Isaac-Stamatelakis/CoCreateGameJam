using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions.Objects {
    public interface IActionSpawnObject
    {
        public IEnumerator moveToTarget(Transform target);
    }
}

