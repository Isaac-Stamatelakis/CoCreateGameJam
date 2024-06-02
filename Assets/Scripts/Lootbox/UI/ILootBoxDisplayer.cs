using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LootBoxes {
    public interface ILootBoxDisplayer 
    {
        public Vector3 getLootBoxPosition();
        public void rebuild();
    }
}

