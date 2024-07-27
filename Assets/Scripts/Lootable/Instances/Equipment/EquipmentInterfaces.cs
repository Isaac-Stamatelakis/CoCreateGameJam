using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;

namespace Items {
    public interface IMoveActionEquipment {
        public void execute(List<CreatureInCombat> creetureInCombats);
    }
}