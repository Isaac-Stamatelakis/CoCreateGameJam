using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Levels.Forge;
using UnityEngine.UI;
using TMPro;
using UI.Inventory;

namespace Crafting.UI {
    public class ForgeSceneUIController : MonoBehaviour, IDisplayController
    {
        [SerializeField] private RecipeUIController recipeUIController;
        [SerializeField] private Transform detailedViewContainer;
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private Button backButton;
        [SerializeField] private Button tabSwitch;
        private ForgeLevel forgeLevel;

        public Transform getDetailedDisplayContainer()
        {
            return detailedViewContainer;
        }

        public void initalize(ForgeLevel forgeLevel) {
            this.forgeLevel = forgeLevel;
            recipeUIController.display(forgeLevel.getRecipes());
        }
    }
}

