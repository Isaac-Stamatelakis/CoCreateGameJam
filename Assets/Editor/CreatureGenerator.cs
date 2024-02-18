using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using CreatureModule;

public class CreatureGenerator : EditorWindow {
    private Sprite baseSprite;
    private Sprite shinySprite;
    private string creatureName;
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
        shinySprite= EditorGUILayout.ObjectField("Base Sprite", shinySprite, typeof(Sprite), true) as Sprite;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name:", GUILayout.Width(70));
        creatureName = EditorGUILayout.TextField(creatureName);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        if (GUILayout.Button("Generate"))
        {
            generate();
        }
    }

    void generate()
    {
        Creeture creeture = ScriptableObject.CreateInstance<Creeture>();
        creeture.name = creatureName;
        

        /*
        TileItem tileItem = ScriptableObject.CreateInstance<TileItem>();
        StandardTile tile = ScriptableObject.CreateInstance<StandardTile>();
        tile.sprite = sprite;
        tile.colliderType = Tile.ColliderType.Grid;
        Vector2Int spriteSize = Global.getSpriteSize(sprite);
        Matrix4x4 tileTransform = tile.transform;
        if (spriteSize.x % 2 == 0) {
            tileTransform.m03 = 0.25f;
        }
        if (spriteSize.y % 2 == 0) {
            tileTransform.m13 = 0.25f;
        }
        tile.transform = tileTransform;
        
        tile.name = "T~" + tileName;
        tileItem.name = tileName;
        tileItem.tile = tile;

        string path = "Assets/EditorCreations/" + tileName + "/";
        if (AssetDatabase.IsValidFolder(path)) {
            Debug.LogError("Tile Generation for "+  tileItem + "Abanadoned as Folder already exists at EditorCreations");
            return;
        }
        AssetDatabase.CreateFolder("Assets/EditorCreations", tileName);
        
        tileItem.id = tileName;
        tileItem.id = tileItem.id.ToLower().Replace(" ","_");
        tile.id = tileItem.id;
        
        AssetDatabase.CreateAsset(tile, path + tile.name + ".asset");
        AssetDatabase.CreateAsset(tileItem, path + tileItem.name + ".asset");
        Debug.Log("TileItem and Tile Created for " + tileName + " at " + path);
        */
    }
}
