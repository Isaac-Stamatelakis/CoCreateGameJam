using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalUtils
{
    public static void deleteChildren(Transform element) {
        for (int i = 0; i < element.childCount; i++) {
            GameObject.Destroy(element.GetChild(i).gameObject);
        }
    }
}
