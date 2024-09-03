using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LootBoxes {
    [CreateAssetMenu(fileName = "New Lootbox", menuName = "Item/Lootbox/Animator")]
    public class StandardAnimatedLootBox : LootBox
    {
        [SerializeField] private RuntimeAnimatorController animatorController;

        public RuntimeAnimatorController AnimatorController { get => animatorController; }
    }
}

