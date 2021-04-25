using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryInfoCard : MonoBehaviour
{

    public ProgressSlider environmentSlider, sentimentSlider, oilStorageSlider;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// Accepts a PrimaryInfoCardResourcesDollarsUpdate
    public void DollarsDidChange(PubSubListenerEvent e) {
        PrimaryInfoCardResourcesDollarsUpdate update = (PrimaryInfoCardResourcesDollarsUpdate)e.value;
    }

    /// PrimaryInfoCardResourcesOilUpdate
    public void OilDidChange(PubSubListenerEvent e) {
        PrimaryInfoCardResourcesOilUpdate update = (PrimaryInfoCardResourcesOilUpdate)e.value;
    }

    /// Accepts a PrimaryInfoCardResourcesEquipmentUpdate
    public void EquipmentDidChange(PubSubListenerEvent e) {
        PrimaryInfoCardResourcesEquipmentUpdate update = (PrimaryInfoCardResourcesEquipmentUpdate)e.value;
    }

    // Accepts a double
    public void EvironmentalChange(PubSubListenerEvent e) {
        environmentSlider.progress = ((float) e.value) / 100f;
    }

    // Accepts a double
    public void PublicSentimentDidChange(PubSubListenerEvent e) {
        sentimentSlider.progress = ((float) e.value) / 100f;
    }

    // Accepts a double
    public void OilStorageDidChange(PubSubListenerEvent e) {
        oilStorageSlider.progress = ((float) e.value) / 100f;
    }
}

public struct PrimaryInfoCardResourcesDollarsUpdate
{
    public decimal dollarsAmount, dollarsIncomePerSecond, dolarsExpensesPerSecond;
}

public struct PrimaryInfoCardResourcesOilUpdate
{
    public decimal oilPrice, oilProduced, oilConsumed;
}

public struct PrimaryInfoCardResourcesEquipmentUpdate
{
    public int numberOfDerricks, numberOfAllowedDerricks;
    public int numberOfRigs, numberOrAllowedRigs;
    public int numberOfTrucks, numberOfAllowedTrucks;
    public int numberOfShips, numberOfAllowedShips;
}