using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Actions.Script;
using System;

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

    private readonly int PADDING = 20;
    private readonly int SPACE_PER_LINE = 15;
    private readonly int LINE_PADDING = 2;
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
        setDescription();
        
    }
    
    private void setDescription() {
        try {
            description = scriptedAction.getDescription();
        } catch (Exception e) {
            description = $"Invalid Script\n{e.Message}";
            throw new Exception(e.Message,e);
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (GUILayout.Button("Reload Description"))
        {
            EditorGUILayout.TextArea(scriptedAction.getDescription(), GUILayout.MinHeight(150));
        }
        GUILayout.Space(PADDING);
        string[] splitLines = actionScriptProp.stringValue.Split('\n');
        int lineCount = splitLines.Length;
        EditorGUILayout.LabelField("Action Script", EditorStyles.boldLabel);
        actionScriptProp.stringValue = EditorGUILayout.TextArea(actionScriptProp.stringValue, GUILayout.MinHeight((lineCount+LINE_PADDING)*SPACE_PER_LINE));

        GUILayout.Space(PADDING);

        EditorGUILayout.LabelField("Description", EditorStyles.boldLabel);
        GUIStyle textAreaStyle = new GUIStyle(EditorStyles.textArea);
        textAreaStyle.wordWrap = true;

        GUI.enabled = false;
        EditorGUILayout.TextArea(description, textAreaStyle,GUILayout.MinHeight(100));
        GUI.enabled = true;

        EditorGUILayout.LabelField("Pre Select Description", EditorStyles.boldLabel);
        preSelectDescription.stringValue = EditorGUILayout.TextArea(preSelectDescription.stringValue, GUILayout.MinHeight(25));
        EditorGUILayout.LabelField("Select Description", EditorStyles.boldLabel);
        selectDescription.stringValue = EditorGUILayout.TextArea(selectDescription.stringValue, GUILayout.MinHeight(25));
        EditorGUILayout.LabelField("Post Select Description", EditorStyles.boldLabel);
        postSelectDescription.stringValue = EditorGUILayout.TextArea(postSelectDescription.stringValue, GUILayout.MinHeight(25));

        GUILayout.Space(PADDING);

        EditorGUILayout.PropertyField(animationsProp, true);
        EditorGUILayout.PropertyField(prefabsProp, true);

        serializedObject.ApplyModifiedProperties();
    }
}
