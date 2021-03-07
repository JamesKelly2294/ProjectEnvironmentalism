using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if (UNITY_EDITOR) 
using UnityEditor;
#endif

public class AlertManager : MonoBehaviour
{

    public GameObject AlertTarget;

    public GameObject AlertWrapperPrefab;

    public AudioSource AudioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject SummonNotification(GameObject prefab, string title, string description, Sprite icon, Color iconTint, bool doesAutoDismiss = true, float autoDismissTime = 10f, AlertWasDismissedDelegate wasDismissed = null, AudioClip sound = null) {
        GameObject wrapper = SummonNotification(prefab);
        Alert alert = wrapper.GetComponentInChildren<Alert>();
        alert.Configure(title, description, icon, iconTint, doesAutoDismiss, autoDismissTime, wasDismissed, sound);

        return wrapper;
    }

    public GameObject SummonNotification(GameObject prefab) {
        GameObject wrapper = Instantiate(AlertWrapperPrefab, AlertTarget.transform);
        GameObject obj = Instantiate(prefab, wrapper.transform);
        AlertWrapper alertWrapper = wrapper.GetComponent<AlertWrapper>();
        alertWrapper.Alert = obj;
        alertWrapper.AlertManager = this;

        return wrapper;
    }
}

#if (UNITY_EDITOR) 
// Declare type of Custom Editor
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
        AlertManager alertManager = (AlertManager) target;

        GUILayout.Space(20f);
        GUILayout.Label("Summon Notification", EditorStyles.boldLabel);
        GUILayout.Space(10f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Alert Prefab", GUILayout.Width(labelWidth));
        prefab = (GameObject) EditorGUILayout.ObjectField(prefab, typeof(GameObject), true, GUILayout.Width(labelWidth));
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
        sprite = (Sprite) EditorGUILayout.ObjectField(sprite, typeof(Sprite), true, GUILayout.Width(labelWidth));
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
        sound = (AudioClip) EditorGUILayout.ObjectField(sound, typeof(AudioClip), true, GUILayout.Width(labelWidth));
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
#endif