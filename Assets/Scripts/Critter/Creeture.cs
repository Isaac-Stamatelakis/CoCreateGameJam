using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootBoxModule;
using InventoryModule;

namespace CreatureModule {
    [CreateAssetMenu(fileName = "New Level Creeture", menuName = "Creeture")]
    [System.Serializable]
    public class Creeture : Lootable, IItem
    {
        public Sprite sprite;
        public string id;
        public int speed;
        public int strength;
        public int health;
    }
    [System.Serializable]
    public class EquipedCreeture {
        private int currentHealth;
        private bool IsDead{get => CurrentHealth <= 0;}
        public int CurrentHealth { get => currentHealth; set => currentHealth = value; }

        public Creeture creeture;
        public List<Equipment> equipment;
        public EquipedCreeture(Creeture creeture, List<Equipment> equipment) {
            this.creeture = creeture;
            this.equipment = equipment;
        }

        public int getSpeed() {
            int speed = creeture.speed;
            foreach (Equipment equipment in equipment) {
                if (equipment.type != EquipmentType.Speed) {
                    continue;
                }
                speed += (int) Mathf.Pow(2,(int)equipment.rarity);
            }
            return speed;
        }

        public int getAttacks() {
            int attacks = creeture.strength;
            foreach (Equipment equipment in equipment) {
                if (equipment.type != EquipmentType.Attack) {
                    continue;
                }
                attacks += (int) Mathf.Pow(2,(int)equipment.rarity+1);
            }
            return attacks;
        }

        public int getHealth() {
            int health = creeture.health;
            foreach (Equipment equipment in equipment) {
                if (equipment.type != EquipmentType.Health) {
                    continue;
                }
                health += (int) Mathf.Pow(2,(int)equipment.rarity+1);
            }
            return health;
        }
        public int getHealing() {
            int heal = 1;
            foreach (Equipment equipment in equipment) {
                if (equipment.type != EquipmentType.Health) {
                    continue;
                }
                heal += (int)equipment.rarity+1;
            }
            return heal;
        }
        public int getArmor() {
            int armor = 0;
            foreach (Equipment equipment in equipment) {
                if (equipment.type != EquipmentType.Armor) {
                    continue;
                }
                armor += (int) Mathf.Pow(2,(int)equipment.rarity+1);
            }
            return armor;
        }

        public int getAbility() {
            int ability = 0;
            foreach (Equipment equipment in equipment) {
                if (equipment.type != EquipmentType.Ability) {
                    continue;
                }
                ability += ((int)equipment.rarity+1)!;
            }
            return ability;
        }

        public void heal() {
            CurrentHealth += getHealing();
        }
        public void attack(int damage) {
            CurrentHealth-=damage;
        }
        public void initForBattle() {
            CurrentHealth = getHealth();
        }
    }
}

