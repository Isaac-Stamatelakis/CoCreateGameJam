using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Levels.Combat;

namespace Creatures.Actions {
    [CreateAssetMenu(fileName = "New Move", menuName = "Creeture/Moves/Attack")]
    public class SingleAttack : CreatureAction
    {
        [SerializeField] private float damage;
        [SerializeField] private DamageType damageType;
        public override void execute(CreatureSelector selector)
        {
            foreach (CreatureCombatObject creatureCombatObject in selector.GetCreatures()) {
                CreatureInCombat creatureInCombat = creatureCombatObject.CreatureInCombat;
                creatureInCombat.hit(damage,damageType);
            }
        }

        public override string getDescription()
        {
            return $"{description} dealing {damage} {damageType} damage";
        }
    }
}

