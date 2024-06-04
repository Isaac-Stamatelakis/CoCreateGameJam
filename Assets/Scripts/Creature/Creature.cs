using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootBoxes;
using Items;
using Creatures.Actions;

namespace Creatures {
    [CreateAssetMenu(fileName = "New Creeture", menuName = "Creeture/Instance")]
    [System.Serializable]
    public class Creature : Lootable, IItem
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private string id;
        [SerializeField] private int speed;
        [SerializeField] private int strength;
        [SerializeField] private int health;
        [SerializeField] private int mana;
        [SerializeField] private string description;
        [SerializeField] private RuntimeAnimatorController controller;
        [SerializeField] private List<DamageType> strengths;
        [SerializeField] private List<DamageType> weaknesses;
        [SerializeField] private List<CreatureAction> moves;
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
        public List<DamageType> Strengths { get => strengths; }
        public List<DamageType> Weaknesses { get => weaknesses; }
        public RuntimeAnimatorController AnimationController { get => controller; }

        public override Sprite getSprite()
        {
            return Sprite;
        }
    }
}

