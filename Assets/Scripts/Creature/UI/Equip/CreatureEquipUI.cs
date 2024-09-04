using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Items.Equipment;
using Items.Equipment.UI;
using Creatures.UI;
using Player;


namespace UI.CreatureEquip {
    public class CreatureEquipUI : MonoBehaviour
    {
        [SerializeField] private EquipmentListSorterUI equipmentListSorter;
        [SerializeField] private CreatureDetailedDisplay creatureDetailedDisplay;
        private EquipedCreature displayedCreature;
        public void display(EquipedCreature equipedCreature, bool onTeam) {
            equipmentListSorter.initalize(PlayerIO.Instance.Equipment);
            creatureDetailedDisplay.display(equipedCreature);
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

