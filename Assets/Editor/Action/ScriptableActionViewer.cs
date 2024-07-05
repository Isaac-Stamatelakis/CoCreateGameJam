using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Actions.Script;
using System;

// Make sure your custom editor is within an Editor folder
[CustomEditor(typeof(ScriptedAction))]
public class ScriptableActionViewer : Editor
{
    SerializedProperty animationsProp;
    SerializedProperty prefabsProp;
    SerializedProperty actionScriptProp;
    SerializedProperty preSelectDescription;
    SerializedProperty selectDescription;
    SerializedProperty postSelectDescription;
    ScriptedAction scriptedAction;
    string description;
    private void OnEnable()
    {
        animationsProp = serializedObject.FindProperty("animations");
        prefabsProp = serializedObject.FindProperty("prefabs");
        actionScriptProp = serializedObject.FindProperty("actionScript");
        preSelectDescription = serializedObject.FindProperty("preSelectDescription");
        selectDescription = serializedObject.FindProperty("selectDescription");
        postSelectDescription = serializedObject.FindProperty("postSelectDescription");
        scriptedAction = (ScriptedAction) target;
        description = "Invalid Script";
        description = scriptedAction.getDescription();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (GUILayout.Button("Reload Description"))
        {
            EditorGUILayout.TextArea(scriptedAction.getDescription(), GUILayout.MinHeight(50));
        }

        EditorGUILayout.LabelField("Description", EditorStyles.boldLabel);
        GUIStyle textAreaStyle = new GUIStyle(EditorStyles.textArea);
        textAreaStyle.wordWrap = true;
        EditorGUILayout.TextArea(description, textAreaStyle,GUILayout.MinHeight(100));

        EditorGUILayout.LabelField("Pre Select Description", EditorStyles.boldLabel);
        preSelectDescription.stringValue = EditorGUILayout.TextArea(preSelectDescription.stringValue, GUILayout.MinHeight(25));
        EditorGUILayout.LabelField("Select Description", EditorStyles.boldLabel);
        selectDescription.stringValue = EditorGUILayout.TextArea(selectDescription.stringValue, GUILayout.MinHeight(25));
        EditorGUILayout.LabelField("Post Select Description", EditorStyles.boldLabel);
        postSelectDescription.stringValue = EditorGUILayout.TextArea(postSelectDescription.stringValue, GUILayout.MinHeight(25));



        EditorGUILayout.PropertyField(animationsProp, true);
        EditorGUILayout.PropertyField(prefabsProp, true);

        EditorGUILayout.LabelField("Action Script", EditorStyles.boldLabel);
        actionScriptProp.stringValue = EditorGUILayout.TextArea(actionScriptProp.stringValue, GUILayout.MinHeight(2500));

        serializedObject.ApplyModifiedProperties();
    }
}
