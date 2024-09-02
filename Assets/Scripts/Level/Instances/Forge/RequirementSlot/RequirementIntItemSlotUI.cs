using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Lists;
using TMPro;
using UnityEngine.UI;

namespace Crafting.UI {
    public class RequirementIntItemSlotUI : UIInventoryDisplayer<RequirementIntItemSlot>
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;
        public override void display(RequirementIntItemSlot element)
        {
            image.sprite = element.getSprite();
            text.text = $"{element.CurrentAmount}/{element.Amount}";
            if (element.CurrentAmount >= element.Amount) {
                text.color = GlobalUtils.fromHex("00B01B");
            } else {
                text.color = Color.red;
            }
        }
    }

}
