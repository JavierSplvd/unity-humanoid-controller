using UnityEngine;
using UnityEditor;

public class FollowObjectEditor : Editor
{
    SerializedProperty targetObject;
    
    void OnEnable()
    {
        targetObject = serializedObject.FindProperty("targetObject");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(targetObject);
        serializedObject.ApplyModifiedProperties();
    }
}
