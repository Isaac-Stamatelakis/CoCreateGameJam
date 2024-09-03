using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using UnityEngine.UI;

namespace UI {
    public class CreatureSelectUI : MonoBehaviour
    {
        [SerializeField] private CreatureSelectListUI creatureSelectListUI;
        [SerializeField] private Button backButton;
        private DisplayableClickCallback callback;
        private List<EquipedCreature> creatures;
        private List<EquipedCreature> displayedCreatures;
        public void display(List<EquipedCreature> creatures, DisplayableClickCallback callback) {
            this.creatures = creatures;
            this.callback = callback;
            displayedCreatures = creatures;
            creatureSelectListUI.display(displayedCreatures);
            backButton.onClick.AddListener(() => {
                GameObject.Destroy(gameObject);
            });
        }

        public void select(int index) {
            EquipedCreature selectedCreature = displayedCreatures[index];
            callback(creatures.IndexOf(selectedCreature));
            GameObject.Destroy(gameObject);
        }
        
    }
}

