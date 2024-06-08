using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Actions.Script;

// Make sure your custom editor is within an Editor folder
[CustomEditor(typeof(ScriptedAction))]
public class ScriptableActionViewer : Editor
{
    SerializedProperty animationsProp;
    SerializedProperty prefabsProp;
    SerializedProperty actionScriptProp;

    private void OnEnable()
    {
        animationsProp = serializedObject.FindProperty("animations");
        prefabsProp = serializedObject.FindProperty("prefabs");
        actionScriptProp = serializedObject.FindProperty("actionScript");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(animationsProp, true);
        EditorGUILayout.PropertyField(prefabsProp, true);

        EditorGUILayout.LabelField("Action Script");
        actionScriptProp.stringValue = EditorGUILayout.TextArea(actionScriptProp.stringValue, GUILayout.MinHeight(2500));

        serializedObject.ApplyModifiedProperties();
    }
}
