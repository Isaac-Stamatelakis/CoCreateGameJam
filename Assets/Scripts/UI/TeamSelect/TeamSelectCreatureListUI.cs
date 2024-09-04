using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Lists;
using Creatures;
using Player;
using UI.CreatureEquip;

namespace UI.TeamSelect {
    public class TeamSelectCreatureListUI : InventoryUI<EquipedCreature>
    {
        [SerializeField] private CreatureSelectUI creatureSelectUIPrefab;
        [SerializeField] private CreatureEquipUI creatureEquipUIPrefab;
        private int selectedIndex = -1;
        public override void leftClick(int index)
        {
            EquipedCreature equipedCreature = elements[index];
            TeamSelectUI teamSelectUI = GlobalUtils.getComponentInHeirarchy<TeamSelectUI>(transform);
                
            if (equipedCreature == null) {
                selectedIndex = index;
                CreatureSelectUI creatureSelectUI = UIUtils.instantiateUIPrefab<CreatureSelectUI>(creatureSelectUIPrefab,teamSelectUI.transform);
                creatureSelectUI.display(PlayerIO.Instance.EquipedCreetures,nullSelectCallback);
            } else {
                CreatureEquipUI creatureEquipUI = UIUtils.instantiateUIPrefab<CreatureEquipUI>(creatureEquipUIPrefab,teamSelectUI.transform);
                creatureEquipUI.display(PlayerIO.Instance.getPlayerTeam()[index],true);
            }
        }

        private void nullSelectCallback(int index) {
            if (selectedIndex < 0) {
                return;
            }
            elements[selectedIndex] = PlayerIO.Instance.EquipedCreetures[index];
            List<EquipedCreature> playerTeam = PlayerIO.Instance.getPlayerTeam();
            playerTeam[selectedIndex] = elements[selectedIndex];
            PlayerIO.Instance.EquipedCreetures.RemoveAt(index);
            display(elements);
        }

        public override void rightClick(int index)
        {
            
        }
    }
}

