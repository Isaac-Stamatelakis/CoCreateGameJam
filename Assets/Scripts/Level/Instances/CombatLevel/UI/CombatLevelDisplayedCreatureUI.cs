using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UI;
using UI.Inventory;
using Items;
using Creatures;

namespace Levels.Combat {
    public class CombatLevelDisplayedCreatureUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Image image;
        [SerializeField] private SliderTextDisplayUI health;
        [SerializeField] private SliderTextDisplayUI mana;
        [SerializeField] private InventoryUI<Equipment> equipmentInventory;
        [SerializeField] private Button databaseButton;

        public void display(CreatureInCombat creatureInCombat) {
            nameText.text = creatureInCombat.EquipedCreeture.creeture.name;
            image.sprite = creatureInCombat.EquipedCreeture.creeture.getSprite();
        }
    }
}

