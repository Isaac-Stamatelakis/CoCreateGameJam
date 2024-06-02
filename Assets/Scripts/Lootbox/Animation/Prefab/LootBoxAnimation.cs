using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LootBoxes {
    public abstract class LootBoxAnimation : MonoBehaviour
    {
        public abstract IEnumerator execute();
    }
}

