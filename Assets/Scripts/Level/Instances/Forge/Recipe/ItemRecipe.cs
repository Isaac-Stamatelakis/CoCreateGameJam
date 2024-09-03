using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

namespace Crafting.Recipes {
    [CreateAssetMenu(fileName = "New Item Recipe", menuName = "Crafting/Recipe/Item")]
    public class ItemRecipe : Recipe
    {
        [SerializeField] private List<IntItemSlot> inputs;
        [SerializeField] private IntItemSlot output;

        public IntItemSlot Output { get => output;}

        public override List<IntItemSlot> getIntItemSlotInputs()
        {
            return inputs;
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

        public override string getSubText()
        {
            if (output==null) {
                return null;
            }
            return output.amountToString();
        }
    }

}
