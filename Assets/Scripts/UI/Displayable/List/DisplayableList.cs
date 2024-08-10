using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Displayables {
    public class DisplayableList<T> : MonoBehaviour where T : IDisplayable
    {
        [SerializeField] private UIDisplayer<T> elementPrefab;
        [SerializeField] private GridLayoutGroup list;
        public void display(List<IDisplayable> displayables, DisplayableClickCallback callback) {
            GlobalUtils.deleteChildren(list.transform);
            for (int i = 0; i < displayables.Count; i++)
            {
                UIDisplayer<T> listElement = GameObject.Instantiate(elementPrefab);
                listElement.initalize(displayables[i],callback,i,null,list.transform);
                listElement.transform.SetParent(list.transform,false);
            }
        }
    }

}
