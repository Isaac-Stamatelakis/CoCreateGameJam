using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GlobalUtils
{
    public static void deleteChildren(Transform element) {
        for (int i = 0; i < element.childCount; i++) {
            GameObject.Destroy(element.GetChild(i).gameObject);
        }
    }
    public static T stringToEnum<T>(string val) where T : Enum {
        foreach (T enumVal in Enum.GetValues(typeof(T))) {
            if (enumVal.ToString().ToLower().Equals(val)) {
                return enumVal;
            }
        }
        return default(T);
    }
}
