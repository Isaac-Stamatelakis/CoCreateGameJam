using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Levels.Combat;

namespace Actions.Script {
    public class EquipmentActionExecutor
    {
        private CreatureCombatObject attacker;
        private CreatureCombatObject healer;
        private CreatureCombatObject healed;
        private CreatureCombatObject attacked;

        public void setAttack(CreatureCombatObject attacker, CreatureCombatObject attacked) {
            this.attacker = attacker;
            this.attacked = attacked;
        }

        public void setHeal(CreatureCombatObject healer, CreatureCombatObject healed) {
            this.healed = healed;
            this.healer = healer;
        }
        public IEnumerator execute() {
            yield return CombatLevelController.Instance.specialActionCoRoutine(
                selfCreature: attacker,
                target: attacked,
                SpecialSelectTarget.Target
            );
            yield return CombatLevelController.Instance.specialActionCoRoutine(
                selfCreature: attacked,
                target: attacker,
                SpecialSelectTarget.Attacker
            );
            yield return CombatLevelController.Instance.specialActionCoRoutine(
                selfCreature: healed,
                target: healer,
                SpecialSelectTarget.Healer
            );
        }


    }
}

