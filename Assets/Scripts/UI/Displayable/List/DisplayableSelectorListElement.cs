using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UI.Inventory;

namespace UI.Displayables {
    public class DisplayableSelectorListElement : UIDisplayer<IDisplayable>
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI header1;
        [SerializeField] private TextMeshProUGUI header2;
        [SerializeField] private TextMeshProUGUI header3;

        public override void display(IDisplayable displayable, DisplayListParameters displayListParameters)
        {
            image.sprite = displayable.getSprite();
            header1.text = displayable.getName();
            
            if (displayable is ISecondaryHeaderDisplayable secondaryHeaderDisplayable) {
                header2.text = secondaryHeaderDisplayable.getSecondaryHeader();
            } else {
                header2.text = "";
            }
            if (displayable is INumberHeaderDisplayable numberHeaderDisplayable) {
                header3.text = numberHeaderDisplayable.getNumberHeader();
            } else {
                header3.text = "";
            }
        }
    }

}
