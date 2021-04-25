using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DebugMenu))]
[CanEditMultipleObjects]
public class DebugMenuEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DebugMenu debugMenu = (DebugMenu)target;
        if (GUILayout.Button("Build Oil Extractor"))
        {
            debugMenu.BuildOilExtractor();
        }
        if (GUILayout.Button("Setup Houston Route"))
        {
            debugMenu.SetupTradeRoute("Houston");
        }
        if (GUILayout.Button("Setup New Orleans Route"))
        {
            debugMenu.SetupTradeRoute("New Orleans");
        }
        if (GUILayout.Button("Setup Matamoros Route"))
        {
            debugMenu.SetupTradeRoute("Matamoros");
        }
    }
}