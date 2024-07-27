using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

namespace Crafting.Recipes {
    [CreateAssetMenu(fileName = "New Item Recipe", menuName = "Crafting/Recipe/Item")]
    public class ItemRecipe : Recipe
    {
        [SerializeField] private List<ItemSlot> inputs;
        [SerializeField] private ItemSlot output;

        public override int getAmount()
        {
            if (output==null) {
                return 0;
            }
            return output.Amount;
        }

        public override string getName()
        {
            if (output==null) {
                return null;
            }
            return output.getName();
        }

        public override Sprite getSprite()
        {
            if (output==null) {
                return null;
            }
            return output.getSprite();
        }

        public override List<ItemSlot> getInputs()
        {
            return inputs;
        }

        public override ItemSlot getOutput()
        {
            return output;
        }
    }

}
