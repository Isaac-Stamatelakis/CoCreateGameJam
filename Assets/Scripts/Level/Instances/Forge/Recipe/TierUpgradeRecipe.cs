using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

namespace Crafting.Recipes {
    [CreateAssetMenu(fileName = "New Tiered Recipe", menuName = "Crafting/Recipe/Tiered")]
    public class TierUpgradeRecipe : Recipe
    {
        [SerializeField] private TieredItemType type;
        [SerializeField] private int amountOfType;
        [SerializeField] private Rarity inputTier;
        [SerializeField] private Sprite recipeSprite;
        [SerializeField] private List<IntItemSlot> additonalInputs;

        public TieredItemType LootableType { get => type; }
        public Rarity InputTier { get => inputTier; }
        public int AmountOfType { get => amountOfType; set => amountOfType = value; }

        public override List<IntItemSlot> getIntItemSlotInputs()
        {
            return additonalInputs;
        }

        public override string getName()
        {
            return $"{inputTier+1} {type}";
        }

        public override Sprite getSprite()
        {
            return recipeSprite;
        }

        public override string getSubText()
        {
            return "";
        }
    }
}
public enum TieredItemType {
    Creature,
    Equipment
}