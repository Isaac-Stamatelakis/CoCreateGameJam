using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI.Lists;

namespace Creatures.UI {
    public class InventoryCreatureDetailedView : CreatureDetailedDisplay
    {
        [SerializeField] private CreatureEquipUI creatureEquipUIPrefab;
        [SerializeField] private Button equipButton;
        protected override void addListeners()
        {
            equipButton.onClick.AddListener(() => {
                CreatureEquipUI creatureEquipUI = GameObject.Instantiate(creatureEquipUIPrefab);
                InventoryController inventoryController = GlobalUtils.getComponentInHeirarchy<InventoryController>(transform);
                creatureEquipUI.transform.SetParent(inventoryController.transform,false);
                creatureEquipUI.display(equipedCreature,false,itemInventory);
            });
        }

        protected override void onDismiss()
        {
            
        }
    }
}

