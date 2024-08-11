using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Creatures;
using LootBoxes;
using System;

namespace Items {
    public static class ItemSlotUtils 
    {
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

        public static IntItemSlot fromLootFrequency(LootFrequency lootFrequency) {
            Lootable lootable = lootFrequency.val;
            /*
            if (lootable is IUniqueItem uniqueItem) {
                return new UniqueItemSlot(lootable,lootFrequency.rarity);
            }
            */
            return new IntItemSlot(lootable,lootFrequency.amount);
        }

        public static int getAmountList(List<IntItemSlot> itemSlots, string id) {
            foreach (IntItemSlot itemSlot in itemSlots) {
                if (itemSlot == null || itemSlot.Lootable == null) {
                    continue;
                }
                if (itemSlot.Lootable.getId().Equals(id)) {
                    return itemSlot.Amount;
                }
            }
            return 0;
        }

        public static int getAmountFromList(List<UniqueItemSlot> uniqueItemSlots, string id, Rarity? rarity) {
            int amount = 0;
            foreach (UniqueItemSlot uniqueItemSlot in uniqueItemSlots) {
                if (uniqueItemSlot == null || uniqueItemSlot.Lootable == null) {
                    continue;
                }
                if (!uniqueItemSlot.Lootable.getId().Equals(id)) {
                    continue;
                }
                if (rarity == null || uniqueItemSlot.Lootable is not IRarityItem rarityItem) {
                    amount ++;
                    continue;
                }
                if (rarityItem.getRarity().Equals(rarity)) {
                    amount ++;
                }
            }
            return amount;
        }

        public static double getAmount(List<DoubleItemSlot> itemSlots, string id) {
            foreach (DoubleItemSlot itemSlot in itemSlots) {
                if (itemSlot == null || itemSlot.Lootable == null) {
                    continue;
                }
                if (itemSlot.Lootable.getId().Equals(id)) {
                    return itemSlot.Amount;
                }
            }
            return 0;
        }


        public static void insertList<T>(List<T> items, T item) where T : StackableItemSlot {
            foreach (T value in items) {
                if (value == null) {
                    continue;
                }
                if (value.match(item)) {
                    value.merge(item);
                    return;
                }
            }
            items.Add(item);
        }

        public static T matchList<T>(List<T> items, T item) where T : StackableItemSlot {
            foreach (T value in items) {
                if (value == null) {
                    continue;
                }
                if (value.match(item)) {
                    return value;
                }
            }
            return null;
        }

        public static bool takeAmount<T>(List<T> items, string id, int amount) where T : StackableItemSlot {
            foreach (T value in items) {
                if (value == null || value.Lootable == null || !value.Lootable.getId().Equals(id)) {
                    continue;
                }
                if (value.amountToInt() > amount) {
                    value.subtractInt(amount);
                    return true;
                }
            }
            return false;
        }

        public static bool hasAmount<T>(List<T> items, string id, int amount) where T : StackableItemSlot {
            foreach (T value in items) {
                if (value == null || value.Lootable == null || !value.Lootable.getId().Equals(id)) {
                    continue;
                }
                return value.amountToInt() > amount;
            }
            return false;
        }

        public static bool takeAmount<T>(List<T> items, string id, double amount) where T : StackableItemSlot {
            foreach (T value in items) {
                if (value == null || value.Lootable == null || !value.Lootable.getId().Equals(id)) {
                    continue;
                }
                if (value.amountToDouble() > amount) {
                    value.subtractDouble(amount);
                    return true;
                }
            }
            return false;
        }

        public static bool hasAmount<T>(List<T> items, string id, double amount) where T : StackableItemSlot {
            foreach (T value in items) {
                if (value == null || value.Lootable == null || !value.Lootable.getId().Equals(id)) {
                    continue;
                }
                return value.amountToInt() > amount;
            }
            return false;
        }

        public static List<G> sortList<T,G>(List<G> itemSlots) where T : Lootable where G : ItemSlot {
            List<G> elements = new List<G>();
            foreach (G itemSlot in itemSlots) {
                if (itemSlot == null || itemSlot.Lootable == null || itemSlot.Lootable is not T value) {
                    continue;
                }
                elements.Add(itemSlot);
            }
            return elements;
        }

        public static DoubleItemSlot fromIntItemSlot(IntItemSlot intItemSlot) {
            return new DoubleItemSlot(intItemSlot.Lootable,(double)intItemSlot.Amount);
        }

        public static IntItemSlot fromDoubleItemSlot(DoubleItemSlot intItemSlot) {
            return new IntItemSlot(intItemSlot.Lootable,(int)intItemSlot.Amount);
        }
    }
}

