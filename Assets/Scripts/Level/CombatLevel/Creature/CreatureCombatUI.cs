using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Creatures;

namespace Levels.Combat {
    public class CreatureCombatUI : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider manaSlider;
        [SerializeField] private Image icon;
        public void sync(CreatureInCombat creatureInCombat) {
            
        }
    }
}

