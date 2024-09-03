using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Items.Equipment;
using System;
using System.Linq;
using System.IO;

[CustomEditor(typeof(TieredEquipmentLayout))]
public class TieredItemGeneratorEditor : Editor
{
    private SerializedProperty statType;
    private SerializedProperty growthType;
    private SerializedProperty growthRate;
    private TieredEquipmentLayout tieredEquipmentLayout;
    private List<Rarity> tieredRarities;
    private void OnEnable()
    {
        statType = serializedObject.FindProperty("stat");
        growthRate = serializedObject.FindProperty("growthRate");
        growthType = serializedObject.FindProperty("growthType");
        tieredEquipmentLayout = (TieredEquipmentLayout) target;
        Rarity[] allRarities = (Rarity[])Enum.GetValues(typeof(Rarity));
        tieredRarities = new List<Rarity>();
        foreach (Rarity rarity in allRarities) {
            if (rarity == Rarity.Untiered) {
                continue;
            }
            tieredRarities.Add(rarity);
        }
        if (tieredEquipmentLayout.sprites == null) {
            tieredEquipmentLayout.sprites = new List<Sprite>();
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        if (GUILayout.Button("Rebuild Tiered Items"))
        {
            rebuildItems();
        }
        EditorGUILayout.PropertyField(statType, true);
        EditorGUILayout.PropertyField(growthType, true);
        EditorGUILayout.PropertyField(growthRate, true);
        for (int i = 0; i < tieredRarities.Count; i++) {
            if (tieredEquipmentLayout.sprites.Count < i+1) {
                tieredEquipmentLayout.sprites.Add(null);
            }
            
            EditorGUILayout.LabelField(tieredRarities[i].ToString(), EditorStyles.boldLabel);
            tieredEquipmentLayout.sprites[i] = (Sprite)EditorGUILayout.ObjectField("Sprite", tieredEquipmentLayout.sprites[i], typeof(Sprite), allowSceneObjects: false);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void rebuildItems() {
        string assetFolder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(tieredEquipmentLayout));
        string itemPath = Path.Combine(assetFolder,tieredEquipmentLayout.name);
        if (Directory.Exists(itemPath)) {
            Debug.LogWarning($"Deleted folder {itemPath}");
            Directory.Delete(itemPath,true);
        }
        AssetDatabase.CreateFolder(assetFolder,tieredEquipmentLayout.name);
        AssetDatabase.Refresh();

        for (int i = 0; i < tieredRarities.Count; i++) {
            Sprite sprite = tieredEquipmentLayout.sprites[i];
            if (sprite == null) {
                continue;
            }
            Rarity rarity = tieredRarities[i];
            Equipment equipment = ScriptableObject.CreateInstance<Equipment>();
            equipment.name = $"{rarity} {tieredEquipmentLayout.stat}";
            equipment.Id = equipment.name.ToLower().Replace(" ","_");
            equipment.Sprite = sprite;
            setStat(equipment,tieredEquipmentLayout.stat,getValue(tieredEquipmentLayout,i+1));
            string assetPath = Path.Combine(itemPath,$"{equipment.name}.asset");
            AssetDatabase.CreateAsset(equipment, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private int getValue(TieredEquipmentLayout tieredEquipmentLayout, int i) {
        switch (tieredEquipmentLayout.growthType) {
            case TieredEquipmentGrowthType.Additive:
                return i * tieredEquipmentLayout.growthRate;
            case TieredEquipmentGrowthType.Multiply:
                int x = 1;
                while (i > 0) {
                    i--;
                    x *= tieredEquipmentLayout.growthRate;
                }
                return x;
        }
        return 0;
    }

    private void setStat(Equipment equipment, CreatureStat creatureStat, int value) {
        switch (creatureStat)
        {
            case CreatureStat.Attack:
                equipment.Attack = value;
                break;
            case CreatureStat.Speed:
                equipment.Speed = value;
                break;
            case CreatureStat.Ability:
                equipment.Ability = value;
                break;
            case CreatureStat.Armor:
                equipment.Defense = value;
                break;
            case CreatureStat.Health:
                equipment.Health = value;
                break;
            case CreatureStat.MaxMana:
                equipment.MaxMana = value;
                break;
            case CreatureStat.InitalMana:
                equipment.BonusMana = value;
                break;
            default:
                Debug.LogWarning("Unknown CreatureStat: " + creatureStat);
                break;
        }
    }
}


