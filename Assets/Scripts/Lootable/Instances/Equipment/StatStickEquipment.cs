using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
    [CreateAssetMenu(fileName = "New Equipment", menuName = "Item/StatStick")]
    public class StatStickEquipment : Equipment
    {
        [SerializeField] private List<StatStickValue> values;
        public override int modifyType(CreatureStat type, int val)
        {
            foreach (StatStickValue statStickValue in values) {
                if (statStickValue.equipmentType == type) {
                    val += statStickValue.value;
                }
            }
            return val;
        }
    }

    [System.Serializable]
    public class StatStickValue {
        public CreatureStat equipmentType;
        public int value;
    }

}
