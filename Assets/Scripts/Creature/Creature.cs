using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootBoxes;
using Items;
using Actions.Script;
using Trading;

namespace Creatures {
    [CreateAssetMenu(fileName = "New Creeture", menuName = "Creature")]
    [System.Serializable]
    public class Creature : Lootable
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private int speed;
        [SerializeField] private int strength;
        [SerializeField] private int health;
        [SerializeField] private int mana;
        [SerializeField] private string description;
        [SerializeField] private RuntimeAnimatorController controller;
        [SerializeField] private List<DamageType> strengths;
        [SerializeField] private List<DamageType> weaknesses;
        [SerializeField] private Currency favouriteCurrency;
        [SerializeField] private Currency hatedCurrency;
        #if UNITY_EDITOR
        public void setSprite(Sprite sprite) {
            this.sprite = sprite;
        }
        public void setId(string id) {
            this.id = id;
        }
        public void setSpeed(int speed) {
            this.speed = speed;
        }
        public void setStrength(int strength) {
            this.strength = strength;
        }
        public void setHealth(int health) {
            this.health = health;
        }
        #endif
        public Sprite Sprite {get => sprite;}
        public string Id {get => id;}
        public int Speed {get => speed;}
        public int Strength {get => strength;}
        public int Health {get => health;}
        public int MaxMana {get => mana;}
        public List<DamageType> Strengths { get => strengths; }
        public List<DamageType> Weaknesses { get => weaknesses; }
        public RuntimeAnimatorController AnimationController { get => controller; }
        public override Sprite getSprite()
        {
            return Sprite;
        }
        public override ItemSlotType getItemSlotType()
        {
            return ItemSlotType.Unique;
        }
    }
}

