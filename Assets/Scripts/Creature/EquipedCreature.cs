using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

namespace Creatures {
    [System.Serializable]
    public class EquipedCreeture : IDisplayable {
        public Creature creeture;
        List<Equipment> equipment;
        private string nickname;
        private int level;
        private int xp;
        public Creature Creeture { get => creeture; }
        public List<Equipment> Equipment { get => equipment;}
        public string Nickname { get => nickname; set => nickname = value;}
        public int Level { get => level; }
        public int XP { get => xp; }

        public EquipedCreeture(Creature creeture, List<Equipment> equipment) {
            this.creeture = creeture;
            this.equipment = equipment;
        }
        public int getStat(CreatureStat stat) {
            int value = 0;
            switch (stat) {
                case CreatureStat.Attack:
                    value = creeture.Strength;
                    break;
                case CreatureStat.Speed:
                    value = creeture.Speed;
                    break;
                case CreatureStat.Ability:
                    value = 0;
                    break;
                case CreatureStat.Armor:
                    value = 0;
                    break;
                case CreatureStat.Health:
                    value = creeture.Health;
                    break;
                default:
                    throw new System.Exception($"Did not cover switch case in for {stat}");
            }
            foreach (Equipment item in equipment) {
                if (item == null) {
                    continue;
                }
                value = item.modifyType(stat,value);
            }
            return value;
        }

        public void giveExperience(int experienceGain) {
            this.xp += experienceGain;
            int toLevel = level * level * 100;
            if (xp > toLevel) {
                xp = xp % toLevel;
                level++;
            }

        }

        public Sprite getSprite()
        {
            return creeture.Sprite;
        }
    }
}

