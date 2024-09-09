using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Items;
using Items.Equipment;

namespace Player {
    public static class PlayerIOUtils
    {
        public static void giveLootable(PlayerData playerData, Lootable lootable) {
            if (lootable is Creature creature) {
                playerData.creetures.Add(new EquipedCreature(creature,new List<EnchantedEquipment>()));
            } else if (lootable is Equipment equipment) {
                playerData.equipment.Add(new EnchantedEquipment(equipment,null));
            } else {
                throw new System.Exception($"PlayerIOUtils method 'giveLootable' did not cover case for lootable of type {lootable.GetType()}");
            }
        }

        public static List<EquipedCreature> initalizePlayerTeam() {
            List<EquipedCreature> playerTeam = new List<EquipedCreature>();
            for (int i = 0; i < Global.PLAYER_TEAM_SIZE; i++) {
                playerTeam.Add(null);
            }
            return playerTeam;  
        }
    }
}

