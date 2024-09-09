using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
using Levels.Combat;
using System.Linq;
using Actions.Script;
using Items.Equipment;

namespace Creatures {
    public class CreatureInCombat : ICombatCreature
    {
        private EquipedCreature equipedCreeture;
        private float health;
        private float mana;
        public bool IsDead{get => health <= 0;}
        public float Health { get => health;}
        public EquipedCreature EquipedCreeture { get => equipedCreeture; }
        private HashSet<StatusEffect> statusEffects = new HashSet<StatusEffect>();
        public float Mana { get => mana; }
        private CreatureCombatObject creatureCombatObject;
        public CreatureCombatObject CreatureCombatObject => creatureCombatObject;
        public void setCombatObject(CreatureCombatObject creatureCombatObject) {
            this.creatureCombatObject = creatureCombatObject;
        }
        public CreatureInCombat(EquipedCreature equipedCreeture) {
            this.equipedCreeture = equipedCreeture;
            this.health = equipedCreeture.getStat(CreatureStat.Health);
            //this.health = Mathf.Min(equipedCreeture.Health,equipedCreeture.getStat(CreatureStat.Health));
        }
        public CreatureInCombat(EquipedCreature equipedCreature, float health, float mana, List<StatusEffect> statusEffects, CreatureCombatObject creatureCombatObject) {
            this.equipedCreeture = equipedCreature;
            this.health = health;
            this.mana = mana;
            this.statusEffects = statusEffects.ToHashSet();
            this.creatureCombatObject = creatureCombatObject;
        }
        public void hit(float damage, DamageType damageType) {
            if (equipedCreeture.Creeture.Weaknesses.Contains(damageType)) {
                damage *= Global.STRENGTH_DAMAGE_MODIFIER;
            } else if (equipedCreeture.Creeture.Strengths.Contains(damageType)) {
                damage *= Global.WEAKNESS_DAMAGE_MODIFIER;
            }
            health -= damage;
            if (health <= 0) {
                health = 0;
            }
        }

        public float getStat(CreatureStat creatureStat) {
            float value = equipedCreeture.getStat(creatureStat);
            foreach (StatusEffect statusEffect in statusEffects) {
                if (statusEffect is IStatChangeEffect statChangeEffect) {
                    value = statChangeEffect.modifyStat(value, creatureStat);
                }
            }
            return value;
        }
        public void heal(float healAmount) {
            health = Mathf.Min(getStat(CreatureStat.Health),health+healAmount);
        }
        public void addStatus(StatusEffect statusEffect) {
            this.statusEffects.Add(statusEffect);
        }

        public bool hasStatus(string statusName) {
            foreach (StatusEffect afflictedEffect in statusEffects) {
                if (statusName.Equals(afflictedEffect.getName())) {
                    return true;
                }
            }
            return false;
        }
        public CreatureInCombat deepCopy() {
            return new CreatureInCombat(equipedCreeture,health,mana,statusEffects.ToList(),creatureCombatObject);
        }

        public List<EnchantedEquipment> getEquipment()
        {
            return equipedCreeture.EnchantedEquipment;
        }

        public float getHealth()
        {
            return health;
        }

        public float getHealthPercent()
        {
            return health/equipedCreeture.getStat(CreatureStat.Health);
        }

        public float getMana()
        {
            return mana;
        }

        public float getManaPercent()
        {
            return mana/equipedCreeture.getStat(CreatureStat.MaxMana);
        }
    }
}

