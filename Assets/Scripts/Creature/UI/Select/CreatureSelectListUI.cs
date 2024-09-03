using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Lists;
using Creatures;

namespace UI {
    public class CreatureSelectListUI : InventoryUI<EquipedCreature>
    {
        [SerializeField] private CreatureSelectUI creatureSelectUI;
        public override void leftClick(int index)
        {
            creatureSelectUI.select(index);
        }

        public override void rightClick(int index)
        {
            
        }
    }
}

