using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Lists;
using Creatures;
using Items.Equipment;

namespace UI.CreatureEquip {
    public class CreatureEquipEquipmentListUI : InventoryUI<EnchantedEquipment>
    {
        private CreatureEquipUI creatureEquipUI;
        public void Start() {
            creatureEquipUI = GlobalUtils.getComponentInHeirarchy<CreatureEquipUI>(transform);
            if (creatureEquipUI == null) {
                Debug.LogWarning("Could not find creatureEquipUI in heirarchy");
            }
        }
        public override void leftClick(int index)
        {
            creatureEquipUI.equip(index);
        }

        public override void rightClick(int index)
        {
            
        }
    }
}

