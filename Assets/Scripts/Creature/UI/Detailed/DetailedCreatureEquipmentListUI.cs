using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Lists;
using Items.Equipment;
using UI;

namespace Creatures.UI {
    public class DetailedCreatureEquipmentListUI : InventoryUI<EnchantedEquipment>
    {
        private CreatureDetailedDisplay creatureDetailedDisplay;
        public void Start() {
            creatureDetailedDisplay = GlobalUtils.getComponentInHeirarchy<CreatureDetailedDisplay>(transform);
        }
        [SerializeField] private CreatureEquipmentUIPopup popupPrefab;
        public override void leftClick(int index)
        {
            if (elements[index] == null) {
                return;
            }
    
            CreatureEquipmentUIPopup popup = GameObject.Instantiate(popupPrefab);
            List<InventoryUI<EnchantedEquipment>> listsToRefresh = new List<InventoryUI<EnchantedEquipment>>{
                this
            };
            listsToRefresh.AddRange(creatureDetailedDisplay.SyncedEquipmentLists);
            UIController.Instance.displayPopUp(popup.gameObject,slots[index].gameObject);
            popup.display(listsToRefresh, creatureDetailedDisplay.EquipedCreature,index);
        }

        public override void rightClick(int index)
        {
            
        }
    }
}

