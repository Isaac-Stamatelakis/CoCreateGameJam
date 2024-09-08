using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Levels.Combat;
using Items.Equipment;


public interface ICombatCreature
{
    public void hit(float damage, DamageType damageType);
    public void heal(float amount);
    public void addStatus(StatusEffect statusEffect);
    public bool hasStatus(string statusName);
    public List<EnchantedEquipment> getEquipment();
    public float getHealth();
    public float getHealthPercent();
    public float getMana();
    public float getManaPercent();
    public float getStat(CreatureStat creatureStat);
}


