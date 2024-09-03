using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
using Newtonsoft.Json;
using Items.Equipment;

namespace Creatures {
    public interface IRarityItem {
        public Rarity getRarity();
    }
    [System.Serializable]
    public class EquipedCreature : IDisplayable, IRarityItem {
        [SerializeField] public Creature creeture;
        [SerializeField] private List<EnchantedEquipment> equipment;
        private string nickname;
        private float mood;
        private int level;
        private int xp;
        public Creature Creeture { get => creeture; }
        public List<EnchantedEquipment> EnchantedEquipment { get => equipment;}
        public Rarity rarity;
        public string Nickname { get => nickname; set => nickname = value;}
        public int Level { get => level; }
        public int XP { get => xp; }

        public EquipedCreature(Creature creeture, List<EnchantedEquipment> equipment) {
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
                case CreatureStat.MaxMana:
                    value = creeture.MaxMana;
                    break;
                default:
                    throw new System.Exception($"Did not cover switch case in for {stat}");
            }
            foreach (EnchantedEquipment item in equipment) {
                if (item == null) {
                    continue;
                }
                item.modifyStat(ref value, stat);
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

        public string getName()
        {
            return creeture.name;
        }

        public Rarity getRarity()
        {
            return rarity;
        }
    }
    
    public class EquipCreatureSerializationFactory : SerializationFactory<EquipedCreature, SCreatureData>
    {
        protected override EquipedCreature deserializeValue(SCreatureData sValue)
        {
            LootableRegistry lootableRegistry = LootableRegistry.getInstance();

            Creature creeture = lootableRegistry.getLootable<Creature>(sValue.id);
            if (creeture == null) {
                return null;
            }
            List<EnchantedEquipment> equipment = new EquipmentFactory().deserializeList(sValue.equipmentData);
            return new EquipedCreature(creeture,equipment);
        }
        protected override SCreatureData formatValue(EquipedCreature value)
        {
            if (value == null) {
                return null;
            }
            string creatureId = value.creeture == null ? null : value.creeture.Id;
            string seralizedEquipment = value.EnchantedEquipment == null ? null : new EquipmentFactory().serialize(value.EnchantedEquipment);
            return new SCreatureData(
                creatureId,
                seralizedEquipment
            );
        }
    }

    public class SCreatureData {
            public string id;
            public string equipmentData;
            public SCreatureData(string id, string equipmentData) {
                this.id = id;
                this.equipmentData = equipmentData;
            }
        }

    
}

public abstract class SerializationFactory<T,S> {
        public string serialize(List<T> list) {
            if (list == null) {
                return null;
            }
            List<S> sList = new List<S>();
            foreach (T value in list) {
                if (value == null) {
                    sList.Add(default(S));
                } else {
                    sList.Add(formatValue(value));
                }
            }
            return JsonConvert.SerializeObject(sList);
        }
        public string serialize(T value) {
            return JsonConvert.SerializeObject(formatValue(value));
        }
        public List<T> deserializeList(string json) {
            if (json == null) {
                return null;
            }
            List<S> sList = JsonConvert.DeserializeObject<List<S>>(json);
            List<T> values = new List<T>();
            foreach (S sValue in sList) {
                if (sValue == null) {
                    continue;
                }
                T value = deserializeValue(sValue);
                values.Add(value);
            }
            return values;
        }
        public T deserialize(string json) {
            S sValue = JsonConvert.DeserializeObject<S>(json);
            if (sValue == null) {
                return default(T);
            }
            return deserializeValue(sValue);
        }
        protected abstract T deserializeValue(S sValue);
        protected abstract S formatValue(T value);
    }

