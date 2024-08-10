using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

namespace Crafting.Recipes {
    public interface IRecipe
    {

    }

    public abstract class Recipe : ScriptableObject, IRecipe, IDisplayable
    {
        public abstract string getName();
        public abstract Sprite getSprite();
    }
}

