using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using Creatures;

public class AnimationCreator : EditorWindow {
    private string savePath = "Assets/EditorCreations";
    private Texture2D spriteSheet;
    [MenuItem("Tools/Sprite/Animation")]
    public static void ShowWindow()
    {
        AnimationCreator window = (AnimationCreator)EditorWindow.GetWindow(typeof(AnimationCreator));
        window.titleContent = new GUIContent("Creeture Generator");
    }
    
    void OnGUI()
    {
        spriteSheet = EditorGUILayout.ObjectField("SpriteSheet",spriteSheet,typeof(Texture2D),true) as Texture2D;
        
        EditorGUILayout.Space();
        if (GUILayout.Button("Generate"))
        {
            generate();
        }
    }

    void generate()
    {
        
    }
}
