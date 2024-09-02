using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Lists;
using Creatures;
using Player;

namespace UI.TeamSelect {
    public class TeamSelectCreatureListUI : InventoryUI<EquipedCreature>
    {
        [SerializeField] private CreatureSelectUI creatureSelectUIPrefab;
        private int selectedIndex = -1;
        public override void leftClick(int index)
        {
            EquipedCreature equipedCreature = elements[index];
            if (equipedCreature == null) {
                selectedIndex = index;
                CreatureSelectUI creatureSelectUI = GameObject.Instantiate(creatureSelectUIPrefab);
                TeamSelectUI teamSelectUI = GlobalUtils.getComponentInHeirarchy<TeamSelectUI>(transform);
                creatureSelectUI.transform.SetParent(teamSelectUI.transform,false);
                creatureSelectUI.display(PlayerIO.Instance.EquipedCreetures,nullSelectCallback);
            } else {

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

