using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using UI.Inventory;

namespace Crafting.UI {
    public class SelectableItemSlotUI : UIInventoryDisplayer<UniqueItemSlot>, IPointerClickHandler
    {
        [SerializeField] private Image image;
        public override void display(UniqueItemSlot element)
        {
            throw new System.NotImplementedException();
        }
    }
}

