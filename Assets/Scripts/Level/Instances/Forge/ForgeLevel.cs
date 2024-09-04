using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crafting.Recipes;
using Items;
using Levels;

namespace Levels.Forge {
    [CreateAssetMenu(fileName = "New Forger", menuName = "Level/Forge")]
    public class ForgeLevel : LevelObject
    {
        [SerializeField] private List<Recipe> recipes;
        [SerializeField] private List<Recipe> blueprintRecipes;

        public override string getSceneName()
        {
            return Global.FORGE_SCENE_NAME;
        }

        public List<Recipe> getRecipes() {
            return recipes;
        }
    }
}


