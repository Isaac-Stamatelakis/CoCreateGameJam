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

    public static (Vector3,int) speedAndIterationsToMove(Vector3 a, Vector3 b,float velocity) {
        Vector2 direction = a - b;
        float angle = Mathf.Atan2(direction.y,direction.x);
        Vector3 speed = velocity * new Vector3(Mathf.Cos(angle),Mathf.Sin(angle),0);
        float distance = Vector3.Distance(a,b);
        int iterations = Mathf.FloorToInt(distance/velocity);
        return (speed,iterations);
    }
}
