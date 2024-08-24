using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions.Script;

namespace Items.Equipment {
    [CreateAssetMenu(fileName = "New Equipment", menuName = "Equipment")]
    public class Equipment : Lootable, ISecondaryHeaderDisplayable
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private Rarity rarity;
        [SerializeField] private int attack;
        [SerializeField] private int defense;
        [SerializeField] private int ability;
        [SerializeField] private int speed;
        [SerializeField] private int health;
        [SerializeField] private int maxMana;
        [SerializeField] private int bonusMana;
        [SerializeField] private List<ScriptedAction> selectableActions;
        [SerializeField] private ScriptedAction onAttackAction;
        [SerializeField] private ScriptedAction onHitAction;

        

        protected Equipment(string id, Sprite sprite)
        {
            this.id = id;
            this.sprite = sprite;
        }

        public override ItemSlotType getItemSlotType()
        {
            return ItemSlotType.Unique;
        }

        public string getSecondaryHeader() {
            return "";
        }

        public override Sprite getSprite()
        {
            return sprite;
        }

        public int getStat(CreatureStat creatureStat) {
            switch (creatureStat) {
                case CreatureStat.Attack:
                    return attack;
                case CreatureStat.Speed:
                    return speed;
                case CreatureStat.Ability:
                    return ability;
                case CreatureStat.Armor:
                    return defense;
                case CreatureStat.Health:
                    return health;
                case CreatureStat.MaxMana:
                    return maxMana;
                case CreatureStat.InitalMana:
                    return bonusMana;
                default:
                    throw new System.Exception($"method 'getStat' did not cover case for creatureStat {creatureStat}");
            }
        }
    }

    public static class CreatureStatExtension {
        public static string getEquipmentName(this CreatureStat creatureStat) {
            switch (creatureStat) {
                case CreatureStat.Attack:
                    return "Sword";
                case CreatureStat.Speed:
                    return "Boots";
                case CreatureStat.Ability:
                    return "Wand";
                case CreatureStat.Armor:
                    return "Armor";
                case CreatureStat.Health:
                    return "Health Charm";
                case CreatureStat.MaxMana:
                    return "Max Mana Charm";
                case CreatureStat.InitalMana:
                    return "Starting Mana Charm";
            }
            return null;
        }
    }

    
}

public enum Rarity {
    Common,
    Rare,
    Exotic,
    Legendary,
    Untiered
}

public enum CreatureStat {
    Attack,
    Speed,
    Ability,
    Armor,
    Health,
    MaxMana,
    InitalMana
}