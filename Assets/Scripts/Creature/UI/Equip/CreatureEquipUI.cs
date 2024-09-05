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
            this.displayedCreature = equipedCreature;
            creatureDetailedDisplay.display(equipedCreature);
            creatureDetailedDisplay.addSyncedEquipmentList(equipmentListSorter.InventoryUI);
        }

        public void equip(int index) {
            PlayerIOUtils.clamp<EnchantedEquipment>(displayedCreature.EnchantedEquipment,Global.MAX_CREATURE_EQUIPMENT);
            int firstNullIndex = GlobalUtils.getFirstNullIndex<EnchantedEquipment>(displayedCreature.EnchantedEquipment);
            EnchantedEquipment enchantedEquipment = equipmentListSorter.getSelectedElement(index);
            equipmentListSorter.removeSelectedElement(index);
            bool equipmentFull = firstNullIndex == -1;
            if (!equipmentFull) {
                displayedCreature.EnchantedEquipment[firstNullIndex] = enchantedEquipment;
            } else {
                int creatureEquipmentCount = displayedCreature.EnchantedEquipment.Count;
                EnchantedEquipment toRemove = displayedCreature.EnchantedEquipment[creatureEquipmentCount-1];
                displayedCreature.EnchantedEquipment.RemoveAt(creatureEquipmentCount-1);
                for (int i = creatureEquipmentCount - 1; i > 0; i--) {
                    displayedCreature.EnchantedEquipment[i] = displayedCreature.EnchantedEquipment[i - 1];
                }
                displayedCreature.EnchantedEquipment[0] = enchantedEquipment;
                equipmentListSorter.insert(toRemove);
            }
            creatureDetailedDisplay.display(displayedCreature);
        }
    }
}

