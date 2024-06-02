using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
using Levels.Combat;

namespace Creatures {
    public class CreatureInCombat
    {
        private EquipedCreeture equipedCreeture;
        private float health;
        private bool IsDead{get => health <= 0;}
        public float Health { get => health;}
        public EquipedCreeture EquipedCreeture { get => equipedCreeture; set => equipedCreeture = value; }
        private HashSet<StatusEffect> statusEffects = new HashSet<StatusEffect>();
        public CreatureInCombat(EquipedCreeture equipedCreeture) {
            this.EquipedCreeture = equipedCreeture;
            this.health = equipedCreeture.getStat(CreatureStat.Health);
        }
        public void hit(float damage, DamageType damageType) {
            if (equipedCreeture.Creeture.Weaknesses.Contains(damageType)) {
                damage *= Global.STRENGTH_DAMAGE_MODIFIER;
            } else if (equipedCreeture.Creeture.Strengths.Contains(damageType)) {
                damage *= Global.WEAKNESS_DAMAGE_MODIFIER;
            }
            health -= damage;
        }
        public void heal(float healAmount) {
            health = Mathf.Max(equipedCreeture.getStat(CreatureStat.Health),health+healAmount);
        }
        public void addStatusEffect(StatusEffect statusEffect) {
            this.statusEffects.Add(statusEffect);
        }
    }
}

