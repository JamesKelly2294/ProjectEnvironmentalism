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
        if (GUILayout.Button("Setup Miami Route"))
        {
            debugMenu.SetupTradeRoute("Miami");
        }
        if (GUILayout.Button("Setup Tampa Route"))
        {
            debugMenu.SetupTradeRoute("Tampa");
        }
        if (GUILayout.Button("Setup Pensacola Route"))
        {
            debugMenu.SetupTradeRoute("Pensacola");
        }
        if (GUILayout.Button("Setup New Orleans Route"))
        {
            debugMenu.SetupTradeRoute("New Orleans");
        }
        if (GUILayout.Button("Setup Houston Route"))
        {
            debugMenu.SetupTradeRoute("Houston");
        }
        if (GUILayout.Button("Setup Matamoros Route"))
        {
            debugMenu.SetupTradeRoute("Matamoros");
        }
        if (GUILayout.Button("Setup Heroica Veracruz Route"))
        {
            debugMenu.SetupTradeRoute("Heroica Veracruz");
        }
        if (GUILayout.Button("Setup Merida Route"))
        {
            debugMenu.SetupTradeRoute("Merida");
        }
    }
}