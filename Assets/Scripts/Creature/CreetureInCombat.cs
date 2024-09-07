using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
using Levels.Combat;

namespace Creatures {
    public class CreatureInCombat
    {
        private EquipedCreature equipedCreeture;
        private float health;
        private float mana;
        public bool IsDead{get => health <= 0;}
        public float Health { get => health;}
        public float HealthPercent { get => health/equipedCreeture.getStat(CreatureStat.Health);}
        public EquipedCreature EquipedCreeture { get => equipedCreeture; }
        private HashSet<StatusEffect> statusEffects = new HashSet<StatusEffect>();
        private CreatureCombatObject creatureCombatObject;
        public CreatureCombatObject CreatureCombatObject {get => creatureCombatObject;}
        public float Mana { get => mana; }
        public float ManaPercent {get => mana/equipedCreeture.getStat(CreatureStat.MaxMana);}
        public CreatureInCombat(EquipedCreature equipedCreeture) {
            this.equipedCreeture = equipedCreeture;
            this.health = equipedCreeture.getStat(CreatureStat.Health);
            //this.health = Mathf.Min(equipedCreeture.Health,equipedCreeture.getStat(CreatureStat.Health));
        }
        public void hit(float damage, DamageType damageType) {
            if (equipedCreeture.Creeture.Weaknesses.Contains(damageType)) {
                damage *= Global.STRENGTH_DAMAGE_MODIFIER;
            } else if (equipedCreeture.Creeture.Strengths.Contains(damageType)) {
                damage *= Global.WEAKNESS_DAMAGE_MODIFIER;
            }
            DamageIndicatorUI damageIndicatorUI = CombatLevelPrefabContainer.Instance.getDamageIndicator();
            Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main,creatureCombatObject.transform.position);
            damageIndicatorUI.display(damage,damageType,screenPosition);
            damageIndicatorUI.transform.SetParent(CombatLevelController.Instance.CanvasTransform);
            if (health <= 0) {
                return;
            }
            health -= damage;
            if (health <= 0) {
                health = 0;
                creatureCombatObject.kill();
            }
            creatureCombatObject.CombatUI.display();
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
        public void syncToObject(CreatureCombatObject creatureCombatObject) {
            this.creatureCombatObject = creatureCombatObject;
        }
        public void heal(float healAmount) {
            health = Mathf.Min(getStat(CreatureStat.Health),health+healAmount);
        }
        public void addStatusEffect(StatusEffect statusEffect) {
            this.statusEffects.Add(statusEffect);
        }

        public bool hasStatusEffect(string statusName) {
            foreach (StatusEffect afflictedEffect in statusEffects) {
                if (statusName.Equals(afflictedEffect.getName())) {
                    return true;
                }
            }
            return false;
        }
    }
}

