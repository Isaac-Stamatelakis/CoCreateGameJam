using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

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

    public static Color fromHex(string hex) {
        bool hashFirst = hex.StartsWith('#');
        bool valid = hashFirst ? hex.Length == 7 || hex.Length == 9 : hex.Length == 6 || hex.Length == 8;
        if (!valid) {
            throw new Exception($"Provided color hex {hex} is not valid");
        }
        if (hashFirst) {
            hex = hex.Substring(1);
        }
        int r = Int32.Parse(hex.Substring(0,2), NumberStyles.HexNumber);
        int g = Int32.Parse(hex.Substring(2,2), NumberStyles.HexNumber);
        int b = Int32.Parse(hex.Substring(4,2), NumberStyles.HexNumber);
        int a = 255;
        bool alphaProvided = hex.Length == 8;
        if (alphaProvided) {
            a = Int32.Parse(hex.Substring(6,2), NumberStyles.HexNumber);
        }
        return new Color(r/255f,g/255f,b/255f,a/255f);
    }

    public static T getComponentInHeirarchy<T>(Transform transform) where T : Behaviour{
        while (transform != null) {
            T component = transform.GetComponent<T>();
            if (component != null) {
                return component;
            }
            transform = transform.parent;
        }
        return null;
    }
}
