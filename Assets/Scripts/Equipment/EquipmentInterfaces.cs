using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;

namespace Items.Equipment {
    public interface IOnAttackEquipment {
        public void execute(List<CreatureInCombat> creetureInCombats);
    }
    public interface IBonusActionEquipment {
        
    }
    public interface IOnDamagedEquipment {

    }
}