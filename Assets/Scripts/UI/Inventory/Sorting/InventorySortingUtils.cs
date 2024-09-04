using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UI.Lists {
    public static class InventorySortingUtils
    {
        public delegate E EnumGetter<in T, out E>(T element) where E : Enum;
        public static List<T> sortSearch<T>(string value, List<T> elements, List<T> displayed, int previousSearchLength) where T : IDisplayable{
            // Only have to search displayed if searchValue is longer 
            List<T> elementsToSort = value.Length > previousSearchLength ? displayed : elements;
            List<T> newDisplayed = new List<T>();
            foreach (T element in elementsToSort) {
                if (element.getName().Contains(value)) {
                    newDisplayed.Add(element);
                }
            }
            return newDisplayed;
        }

        public static List<T> sortEnum<T,E>(List<T> elements, E? enumeration, EnumGetter<T,E> enumGetter) where T : IDisplayable where E : struct,Enum
        {
            if (enumeration == null) {
                return elements;
            }
            List<T> toDisplay = new List<T>();
            foreach (T element in elements) {
                E value = enumGetter(element);
                if (value.Equals(enumeration)) {
                    toDisplay.Add(element);
                }
            }
            return toDisplay;

        }
    }
}

