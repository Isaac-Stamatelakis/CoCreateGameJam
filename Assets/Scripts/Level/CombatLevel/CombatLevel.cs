using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreatureModule;
using Player;
using LootBoxModule;

namespace LevelModule.Combat {
    [CreateAssetMenu(fileName = "New Combat Level", menuName = "Level/CombatLevel")]
    public class CombatLevel : Level
    {
        
        private List<EquipedCreeture> playerCreatures;
        [SerializeField] public List<EquipedCreeture> enemyCreatures;
        [SerializeField] public List<Lootable> lootables;
        public void initalize(List<EquipedCreeture> playerCreatures) {
            this.playerCreatures = playerCreatures;
        }

        public override void load()
        {
            GameObject controller = new GameObject();
            CombatLevelController combatLevelController = controller.AddComponent<CombatLevelController>();
            controller.name = "Combat Level Controller";

            int index = 0;
            Dictionary<EquipedCreeture, Transform> playerCreetures = new Dictionary<EquipedCreeture, Transform>();
            foreach (EquipedCreeture equipedCreeture in playerCreatures) {
                if (index > 2) {
                    Debug.LogError("Only 3 creatures allowed");
                    return;
                }
                Transform cTransform = CreetureSpriteLoader.loadCreature(equipedCreeture, index, Side.Left).transform;
                playerCreetures[equipedCreeture] = cTransform;
                cTransform.SetParent(controller.transform);
                index ++;
            }

            Dictionary<EquipedCreeture, Transform> aiCreetures = new Dictionary<EquipedCreeture, Transform>();
            index = 0;
            foreach (EquipedCreeture equipedCreeture in enemyCreatures) {
                if (index > 2) {
                    Debug.LogError("Only 3 creatures allowed");
                    return;
                }
                Transform cTransform = CreetureSpriteLoader.loadCreature(equipedCreeture, index, Side.Right).transform;
                aiCreetures[equipedCreeture] = cTransform;
                cTransform.SetParent(controller.transform);
                index ++;
            }
            combatLevelController.init(playerCreetures,aiCreetures);
            GameObject combatUI = GameObject.Instantiate(Resources.Load<GameObject>("UI/CombatUI"));
            CombatPlayerInputController inputController = combatUI.AddComponent<CombatPlayerInputController>(); 
            inputController.LevelController = combatLevelController;

            
            
        }
    }
}

