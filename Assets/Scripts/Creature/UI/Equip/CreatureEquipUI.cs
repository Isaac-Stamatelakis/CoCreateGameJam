using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Items.Equipment;
using Items.Equipment.UI;
using Creatures.UI;
using Player;
using UI.Lists;
using UnityEngine.UI;
using UI;

namespace Creatures.UI {
    public class CreatureEquipUI : MonoBehaviour
    {
        [SerializeField] private EquipmentListSorterUI equipmentListSorter;
        [SerializeField] private CreatureDetailedDisplay creatureDetailedDisplay;
        [SerializeField] private BackButtonUI backButton;
        private IRefreshableUI parentToRefresh;
        private EquipedCreature displayedCreature;

        public IRefreshableUI ParentToRefresh { get => parentToRefresh; }

        public void Start() {
            backButton.GetComponent<Button>().onClick.AddListener(() => {
                ParentToRefresh.refresh();
            });
        }
        public void display(EquipedCreature equipedCreature, bool onTeam, IRefreshableUI refreshableUI) {
            equipmentListSorter.initalize(PlayerIO.Instance.Equipment);
            this.displayedCreature = equipedCreature;
            this.parentToRefresh = refreshableUI;
            creatureDetailedDisplay.setRecruitable(!onTeam);
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
                displayedCreature.EnchantedEquipment.Insert(0,enchantedEquipment);
                equipmentListSorter.insert(toRemove);
            }
            creatureDetailedDisplay.display(displayedCreature);
        }
    }
}

