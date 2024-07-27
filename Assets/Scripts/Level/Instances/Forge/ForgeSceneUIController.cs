using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Levels.Forge;
using UnityEngine.UI;
using TMPro;

namespace Crafting.UI {
    public class ForgeSceneUIController : MonoBehaviour
    {
        [SerializeField] private RecipeUIController recipeUIController;
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private Button backButton;
        [SerializeField] private Button tabSwitch;
        private ForgeLevel forgeLevel;
        public void initalize(ForgeLevel forgeLevel) {
            this.forgeLevel = forgeLevel;
            recipeUIController.display(forgeLevel.getRecipes());
        }
    }
}

