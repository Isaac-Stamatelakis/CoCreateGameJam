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
        private CreatureInCombat creatureInCombat;
        public void sync(CreatureInCombat creatureInCombat) {
            this.creatureInCombat = creatureInCombat;
            display();
        }

        public void display() {
            healthSlider.value=creatureInCombat.Health/creatureInCombat.EquipedCreeture.getStat(Items.CreatureStat.Health);
            manaSlider.value = 1;
        }
    }
}

