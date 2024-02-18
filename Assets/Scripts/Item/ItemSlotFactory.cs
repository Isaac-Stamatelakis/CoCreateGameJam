using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CreatureModule;

namespace InventoryModule {
    public static class ItemSlotFactory 
    {
        public static GameObject fromItem(IItem item) {
            if (item is Equipment equipment) {
                return fromEquipment(equipment);
            }
            if (item is Creeture creeture) {
                return fromCreature(creeture);
            }
            return null;
        }
        public static GameObject fromEquipment(Equipment equipment) {
            GameObject panel = GameObject.Instantiate(Resources.Load<GameObject>("Inventory/Panel"));
            Image image = panel.transform.Find("Image").GetComponent<Image>();
            RectTransform panelTransform = panel.GetComponent<RectTransform>();

            ItemSlotClick itemSlotClick = panel.AddComponent<ItemSlotClick>();
            if (equipment != null) {
                itemSlotClick.Equipment = equipment;
                image.sprite  = equipment.sprite;
                Image panelImage = panel.GetComponent<Image>();
                panelImage.sprite = getSprite(equipment.rarity);
                panelImage.color = getColor(equipment.rarity);
                RectTransform rectTransform = panelImage.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(80,80);
            } else {
                GameObject.Destroy(image);
            }
            return panel;
        }

        public static GameObject fromCreature(Creeture creeture) {
            GameObject panel = GameObject.Instantiate(Resources.Load<GameObject>("Inventory/Panel"));
            Image image = panel.transform.Find("Image").GetComponent<Image>();
            RectTransform panelTransform = panel.GetComponent<RectTransform>();
            if (creeture != null) {
                RectTransform rectTransform = image.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(160,160);
                image.sprite  = creeture.sprite;

            } else {
                GameObject.Destroy(image);
            }
            return panel;
        }

        public static Color getColor(Rarity rarity) {
            switch (rarity) {
                case Rarity.Common:
                    return new Color(1f,1f,1f,100f/255f);
                case Rarity.Exotic:
                    return new Color(1f,1f,1f,150f/255f);
                case Rarity.Rare:
                    return new Color(1f,1f,1f,200f/255f);
                case Rarity.Legendary:
                    return new Color(1f,1f,1f,1f);
            }
            return Color.white;
        }

        public static Sprite getSprite(Rarity rarity) {
            switch (rarity) {
                case Rarity.Common:
                    return Resources.Load<Sprite>("Sprites/Borders/Common_border");
                case Rarity.Exotic:
                    return Resources.Load<Sprite>("Sprites/Borders/Exotic_border");
                case Rarity.Rare:
                    return Resources.Load<Sprite>("Sprites/Borders/Rare_border");
                case Rarity.Legendary:
                    return Resources.Load<Sprite>("Sprites/Borders/Legendary_border");
            }
            return null;
        }
    }
}

