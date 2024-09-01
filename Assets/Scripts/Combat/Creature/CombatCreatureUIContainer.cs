using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels.Combat {
    public class CombatCreatureUIContainer : MonoBehaviour
    {
        [SerializeField] private CreatureCombatUI creatureCombatUIPrefab;
        public void addCreature(CreatureCombatObject creatureCombatObject) {
            CreatureCombatUI creatureCombatUI = GameObject.Instantiate(creatureCombatUIPrefab);
            creatureCombatUI.transform.SetParent(transform,false);
            creatureCombatObject.syncCombatUI(creatureCombatUI);
            creatureCombatUI.sync(creatureCombatObject.CreatureInCombat);
            creatureCombatUI.name = $"{creatureCombatObject.CreatureInCombat.EquipedCreeture.creeture.name} UI";
            
        }
    }
}

