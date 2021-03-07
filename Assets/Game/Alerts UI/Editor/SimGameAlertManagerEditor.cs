using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Declare type of Custom Editor
// This has to be in a folder named "Editor" or Unity will try to include it with the build (breaking the build)
[CustomEditor(typeof(SimGameAlertManager))]
public class SimGameAlertManagerEditor : Editor
{

    // OnInspector GUI
    public override void OnInspectorGUI()
    {

        // Call base class method
        base.DrawDefaultInspector();

        // Custom form for Player Preferences
        SimGameAlertManager alertManager = (SimGameAlertManager)target;

        GUILayout.Space(20f);
        GUILayout.Label("Summon Notifications", EditorStyles.boldLabel);
        GUILayout.Space(10f);


        if (GUILayout.Button("Summon Low Power"))
        {
            alertManager.SummonLowPower();
        }

        if (GUILayout.Button("Summon Good Power"))
        {
            alertManager.SummonGoodPower();
        }

        if (GUILayout.Button("Summon Low Power"))
        {
            alertManager.SummonLowWater();
        }

        if (GUILayout.Button("Summon Good Power"))
        {
            alertManager.SummonGoodWater();
        }
    }
}