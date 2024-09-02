using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UI;
using UI.Inventory;
using Items;
using Creatures;
using Items.Equipment;

namespace Levels.Combat {
    public class CombatLevelDisplayedCreatureUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI strength;
        [SerializeField] private TextMeshProUGUI speed;
        [SerializeField] private TextMeshProUGUI magic;
        [SerializeField] private TextMeshProUGUI defense;
        [SerializeField] private SliderTextDisplayUI health;
        [SerializeField] private SliderTextDisplayUI mana;
        [SerializeField] private InventoryUI<Equipment> equipmentInventory;
        [SerializeField] private Button databaseButton;

        public void display(CreatureInCombat creatureInCombat) {
            nameText.text = creatureInCombat.EquipedCreeture.creeture.name;
            image.sprite = creatureInCombat.EquipedCreeture.creeture.getSprite();
            strength.text = creatureInCombat.EquipedCreeture.getStat(CreatureStat.Attack).ToString();
            speed.text = creatureInCombat.EquipedCreeture.getStat(CreatureStat.Speed).ToString();
            magic.text = creatureInCombat.EquipedCreeture.getStat(CreatureStat.Ability).ToString();
            defense.text = creatureInCombat.EquipedCreeture.getStat(CreatureStat.Armor).ToString();
            health.display(creatureInCombat.Health,creatureInCombat.EquipedCreeture.getStat(CreatureStat.Health));
            mana.display(creatureInCombat.Mana,creatureInCombat.EquipedCreeture.getStat(CreatureStat.MaxMana));
        }
    }
}

