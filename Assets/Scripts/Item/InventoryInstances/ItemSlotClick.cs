using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventoryModule {
    public class ItemSlotClick : MonoBehaviour, IPointerClickHandler
    {
        private bool clicked = false;
        private Equipment equipment;

        public Equipment Equipment { get => equipment; set => equipment = value; }

        public void OnPointerClick(PointerEventData eventData)
        {
            clicked = !clicked;
            Image image = GetComponent<Image>();
            clicked = !clicked;
            if (clicked) {
                image.color = Color.red;
            } else {
                if (equipment != null) {
                    image.color = ItemSlotFactory.getColor(equipment.rarity);
                } else {
                    image.color = new Color(1f,1f,1f,100f/255f);
                }
            }
            
            
        }
    }
}

