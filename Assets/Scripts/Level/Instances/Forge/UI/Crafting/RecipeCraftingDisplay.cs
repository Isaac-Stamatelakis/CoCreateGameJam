using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Inventory;
using Crafting.Recipes;
using UnityEngine.UI;
using TMPro;
using Items;

namespace Crafting.UI {
    public class RecipeCraftingDisplay : UIInventoryDisplayer<Recipe>
    {
        [SerializeField] private Button craftButton;
        [SerializeField] private ItemSlotUI output;
        [SerializeField] private InventoryUI<ItemSlot> inputs;
        [SerializeField] private TextMeshProUGUI outputName;
        public override void display(Recipe element, InventoryUI<Recipe> inventory, int index)
        {
            
        }
    }

}
