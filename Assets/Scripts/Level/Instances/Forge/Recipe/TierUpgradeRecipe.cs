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

        public override int getAmount()
        {
            return 1;
        }

        public override List<ItemSlot> getInputs()
        {
            List<ItemSlot> inputs = new List<ItemSlot>();
            for (int i = 0; i < amount; i++)
            {
                inputs.Add(new SelectableItemSlot(null,1,type,inputTier));
            }
            inputs.AddRange(additonalInputs);
            return inputs;
        }

        public override string getName()
        {
            return $"{inputTier+1} {type}";
        }

        public override ItemSlot getOutput()
        {
            throw new System.NotImplementedException();
        }

        public override Sprite getSprite()
        {
            return recipeSprite;
        }
    }

    public enum TieredItemType {
        Creature,
        Equipment
    }

}
