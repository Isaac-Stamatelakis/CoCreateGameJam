using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Items.Equipment;

namespace UI.CreatureEquip {
    public class CreatureEquipUI : MonoBehaviour
    {
        private EquipedCreature displayedCreature;
        public void display(EquipedCreature equipedCreature, bool onTeam) {

        }

        public void equip(int index) {
            int firstNullIndex = GlobalUtils.getFirstNullIndex<EnchantedEquipment>(displayedCreature.EnchantedEquipment);
            bool equipmentFull = firstNullIndex == -1;
            if (!equipmentFull) {

            } else {

            }
        }
    }
}

