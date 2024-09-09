using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.Equipment {
    public class EnchantmentRegistry
    {
        private static EnchantmentRegistry instance;
        private static Dictionary<string, EquipmentEnchant> dict;
        private EnchantmentRegistry() {
            dict = new Dictionary<string, EquipmentEnchant>();
        }
        public static EquipmentEnchant getEnchantment(string id) {
            if (instance == null) {
                instance = new EnchantmentRegistry();
            }
            if (id == null) {
                return null;
            }
            if (dict.ContainsKey(id)) {
                return dict[id];
            }
            return null;
        }
    }
}

