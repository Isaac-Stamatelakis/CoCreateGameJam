using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Lists;
using UnityEngine.UI;
using Items.Equipment;
using Player;

namespace Creatures.UI {
    public class CreatureEquipmentUIPopup : MonoBehaviour
    {
        [SerializeField] private Button unequipButton;
        [SerializeField] private Button informationButton;

        private EquipedCreature equipedCreature;
        private List<InventoryUI<EnchantedEquipment>> equipmentLists;
        private int index;
        public void Start() {
            unequipButton.onClick.AddListener(() => {
                EnchantedEquipment enchantedEquipment = equipedCreature.EnchantedEquipment[index];
                PlayerIO.Instance.Equipment.Add(enchantedEquipment);
                equipedCreature.EnchantedEquipment[index] = null;
                foreach (InventoryUI<EnchantedEquipment> equipmentList in equipmentLists) {
                    equipmentList.refresh();
                }
                GameObject.Destroy(gameObject);
            });
            informationButton.onClick.AddListener(() => {
                EnchantedEquipment enchantedEquipment = equipedCreature.EnchantedEquipment[index];
                // TODO
                Debug.Log($"Show information for {enchantedEquipment.getName()}");
            });
        }
        public void display(List<InventoryUI<EnchantedEquipment>> listsToRefresh, EquipedCreature element, int index)
        {
            this.index = index;
            this.equipmentLists = listsToRefresh;
            this.equipedCreature = element;
        }
    }
}

