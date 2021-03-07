using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class SimGameAlertManager : MonoBehaviour
{
    public GameObject importantWaterAlert, seriousWaterAlert, importantPowerAlert, seriousPowerAlert;
    public AudioClip importantSound, seriousSound;
    public Sprite doneSprite, errorSprite;
    public Color doneTint, errorTint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SummonLowPower() {
        AlertManager alertManager = GetComponent<AlertManager>();
        alertManager.SummonNotification(seriousPowerAlert, "Not Enough Power", "Your city is not producing enough power to keep up with demand.", errorSprite, errorTint, true, 30, null, seriousSound);
    }

    public void SummonGoodPower() {
        AlertManager alertManager = GetComponent<AlertManager>();
        alertManager.SummonNotification(importantPowerAlert, "Power has been Restored", "", doneSprite, doneTint, true, 5, null, importantSound);
    }

    public void SummonLowWater() {
        AlertManager alertManager = GetComponent<AlertManager>();
        alertManager.SummonNotification(seriousWaterAlert, "Not Enough Water", "Your city is not producing enough water to keep up with demand.", errorSprite, errorTint, true, 30, null, seriousSound);
    }

    public void SummonGoodWater() {
        AlertManager alertManager = GetComponent<AlertManager>();
        alertManager.SummonNotification(importantWaterAlert, "Water has been Restored", "", doneSprite, doneTint, true, 5, null, importantSound);
    }
}

// Declare type of Custom Editor
[CustomEditor(typeof(SimGameAlertManager))]
public class SimGameAlertManagerEditor : Editor 
{

    // OnInspector GUI
    public override void OnInspectorGUI()
    {

        // Call base class method
        base.DrawDefaultInspector();

        // Custom form for Player Preferences
        SimGameAlertManager alertManager = (SimGameAlertManager) target;

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