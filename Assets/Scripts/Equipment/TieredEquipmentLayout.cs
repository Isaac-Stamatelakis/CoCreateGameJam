using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.Equipment {
    [CreateAssetMenu(fileName = "New Equipment", menuName = "Equipment/Tiered Layout")]
    public class TieredEquipmentLayout : ScriptableObject
    {
        public CreatureStat stat;
        public List<Sprite> sprites;
        public TieredEquipmentGrowthType growthType;
        public int growthRate = 2;
    }

    public enum TieredEquipmentGrowthType {
        Additive,
        Multiply
    }
}

