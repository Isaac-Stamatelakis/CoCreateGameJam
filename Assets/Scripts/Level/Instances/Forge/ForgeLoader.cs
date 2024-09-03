using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crafting.UI;

namespace Levels.Forge {
    public class ForgeLoader : LevelLoader<ForgeLevel>
    {
        [SerializeField] private SpriteRenderer background;
        [SerializeField] private SpriteRenderer character;
        [SerializeField] private ForgeSceneUIController forgeSceneUIController;
        protected override void loadLevel(ForgeLevel level)
        {
            forgeSceneUIController.initalize(level);
        }
    }
}

