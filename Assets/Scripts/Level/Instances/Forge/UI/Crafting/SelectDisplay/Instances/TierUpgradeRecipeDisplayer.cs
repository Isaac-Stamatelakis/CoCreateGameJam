using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crafting.Recipes;
using UI.Inventory;
using Items;
using UnityEngine.UI;
using TMPro;
using UI.Displayables;

namespace Crafting.UI {
    public class TierUpgradeRecipeDisplayer : RecipeDisplayer<TierUpgradeRecipe>
    {
        private readonly DisplayListParameters displayListParameters = new DisplayListParameters(
            title: "test",
            primaryColor: GlobalUtils.fromHex("#0AA693"),
            primaryBackgroundColor: GlobalUtils.fromHex("#1D2128"),
            secondaryBackgroundColor: GlobalUtils.fromHex("#1A152A"),
            primaryTextColor: Color.white,
            secondaryTextColor: Color.gray,
            scrollBarColor: GlobalUtils.fromHex("#00B346"),
            new AbsoluteListSize(500),
            new AnchorListSize(0.1f,0.9f)
        );
        [SerializeField] private InventoryUI<RequirementIntItemSlot> recipeInputList;
        [SerializeField] private Button autoFillButton;
        [SerializeField] private TextMeshProUGUI autoFillButtonText;
        [SerializeField] private SelectableItemSlotUI autoFillSelector;
        [SerializeField] private SelectableItemSlotUI output;
        [SerializeField] private InventoryUI<UniqueItemSlot> selectableItems;
        [SerializeField] private ColorableDisplayableList displayableListPrefab;
        private TierUpgradeRecipe tierUpgradeRecipe;
        public override void craft()
        {
            
        }

        public void Start() {
            autoFillSelector.GetComponent<Button>().onClick.AddListener(() => {
                ColorableDisplayableList colorableDisplayableList = GameObject.Instantiate(displayableListPrefab);
                colorableDisplayableList.display(new List<IDisplayable>(),selectFromList,displayListParameters);
                Canvas canvas = GlobalUtils.getCanvas(transform);
                colorableDisplayableList.transform.SetParent(canvas.transform,false);
            });
        }

        public void selectFromList(int index) {

        }

        public override void display(TierUpgradeRecipe element)
        {
            recipeInputList.display(ItemSlotUtils.toRequirementSlots(element.getIntItemSlotInputs()),null);
            switch (tierUpgradeRecipe.LootableType) {
                case TieredItemType.Creature:
                    setButtonText("a Creeture");
                    break;
                case TieredItemType.Equipment:
                    setButtonText("some Equipment");
                    break;
            }
        }

        private void setButtonText(string text) {
            autoFillButtonText.text = $"Select {text} to Autofill";
        }
    }
}

