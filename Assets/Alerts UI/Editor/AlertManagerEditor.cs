using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Declare type of Custom Editor
// This has to be in a folder named "Editor" or Unity will try to include it with the build (breaking the build)
[CustomEditor(typeof(AlertManager))]
public class AlertManagerEditor : Editor
{
    float labelWidth = 150f;


    GameObject prefab;
    string title = "Title";
    string description = "Some Description";
    Sprite sprite;
    Color imageTint = Color.white;
    float autoDismissTime = 5f;
    bool doesAutoDismiss = true;

    public AudioClip sound;


    // OnInspector GUI
    public override void OnInspectorGUI()
    {

        // Call base class method
        base.DrawDefaultInspector();

        // Custom form for Player Preferences
        AlertManager alertManager = (AlertManager)target;

        GUILayout.Space(20f);
        GUILayout.Label("Summon Notification", EditorStyles.boldLabel);
        GUILayout.Space(10f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Alert Prefab", GUILayout.Width(labelWidth));
        prefab = (GameObject)EditorGUILayout.ObjectField(prefab, typeof(GameObject), true, GUILayout.Width(labelWidth));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Title", GUILayout.Width(labelWidth));
        title = GUILayout.TextField(title);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Description", GUILayout.Width(labelWidth));
        description = GUILayout.TextField(description);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Image", GUILayout.Width(labelWidth));
        sprite = (Sprite)EditorGUILayout.ObjectField(sprite, typeof(Sprite), true, GUILayout.Width(labelWidth));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Image Tint", GUILayout.Width(labelWidth));
        imageTint = EditorGUILayout.ColorField(imageTint, GUILayout.Width(labelWidth));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Auto Dismiss", GUILayout.Width(labelWidth));
        doesAutoDismiss = EditorGUILayout.Toggle(doesAutoDismiss, GUILayout.Width(labelWidth));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Auto Dismiss Time", GUILayout.Width(labelWidth));
        autoDismissTime = EditorGUILayout.Slider(autoDismissTime, 1f, 60f, GUILayout.Width(labelWidth));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Auto Dismiss Time", GUILayout.Width(labelWidth));
        sound = (AudioClip)EditorGUILayout.ObjectField(sound, typeof(AudioClip), true, GUILayout.Width(labelWidth));
        GUILayout.EndHorizontal();

        GUILayout.Space(10f);

        if (GUILayout.Button("Summon"))
        {
            alertManager.SummonNotification(prefab, title, description, sprite, imageTint, doesAutoDismiss, autoDismissTime, null, sound);
        }
        if (GUILayout.Button("Summon Raw Prefab"))
        {
            alertManager.SummonNotification(prefab);
        }
    }
}