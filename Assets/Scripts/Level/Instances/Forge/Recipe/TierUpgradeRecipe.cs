using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

namespace Crafting.Recipes {
    [CreateAssetMenu(fileName = "New Tiered Recipe", menuName = "Crafting/Recipe/Tiered")]
    public class TierUpgradeRecipe : Recipe
    {
        [SerializeField] private TieredItemType type;
        [SerializeField] private int amount;
        [SerializeField] private Rarity inputTier;
        [SerializeField] private Sprite recipeSprite;
        [SerializeField] private List<ItemSlot> additonalInputs;
        public override string getName()
        {
            return $"{inputTier+1} {type}";
        }

        public override Sprite getSprite()
        {
            return recipeSprite;
        }
    }
}
public enum TieredItemType {
    Creature,
    Equipment
}