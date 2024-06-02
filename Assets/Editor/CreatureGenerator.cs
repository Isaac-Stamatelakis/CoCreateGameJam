using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using Creatures;

public class CreatureGenerator : EditorWindow {
    private string savePath = "Assets/EditorCreations";
    private Sprite baseSprite;
    private Sprite shinySprite;
    private string creatureName;
    private int health;
    private int damage;
    private int speed;
    [MenuItem("Tools/CreetureGenerator")]
    public static void ShowWindow()
    {
        CreatureGenerator window = (CreatureGenerator)EditorWindow.GetWindow(typeof(CreatureGenerator));
        window.titleContent = new GUIContent("Creeture Generator");
    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        baseSprite = EditorGUILayout.ObjectField("Base Sprite", baseSprite, typeof(Sprite), true) as Sprite;
        shinySprite= EditorGUILayout.ObjectField("Shiny Sprite", shinySprite, typeof(Sprite), true) as Sprite;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name:", GUILayout.Width(70));
        creatureName = EditorGUILayout.TextField(creatureName);
        EditorGUILayout.EndHorizontal();
        health = EditorGUILayout.IntField("Health:", health);
        damage = EditorGUILayout.IntField("Damage:", damage);
        speed = EditorGUILayout.IntField("Speed:", speed);
        GUILayout.FlexibleSpace();
        
        EditorGUILayout.Space();
        if (GUILayout.Button("Generate"))
        {
            generate();
        }
    }

    void generate()
    {
        AssetDatabase.CreateFolder(savePath, creatureName);
        Creature creeture = ScriptableObject.CreateInstance<Creature>();
        creeture.name = creatureName;
        creeture.setHealth(health);
        creeture.setStrength(damage);
        creeture.setSpeed(speed);
        creeture.setSprite(baseSprite);
        creeture.setId(creatureName.ToLower().Replace(" ","_"));
        AssetDatabase.CreateAsset(creeture, savePath + "/" + creatureName + "/" + creeture.name + ".asset");
        Debug.Log("Creeture created " + creeture.name);
        /*
        Creeture shiny = ScriptableObject.CreateInstance<Creeture>();
        shiny.name = "Shiny " + creatureName;
        shiny.Health = health;
        shiny.Strength = damage;
        shiny.Speed = speed;
        shiny.Sprite = baseSprite;
        shiny.Id = "shiny_"+ creatureName.ToLower().Replace(" ","_");
        AssetDatabase.CreateAsset(shiny, savePath + "/" + creatureName + "/" + shiny.name + ".asset");
        Debug.Log("Creeture created " + shiny.name);
        */
    }
}
