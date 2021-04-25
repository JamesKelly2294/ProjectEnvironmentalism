using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class PrimaryInfoCard : MonoBehaviour
{

    public TextMeshProUGUI buisnessName, date;
    public TextMeshProUGUI dollars, income, expenses;
    public TextMeshProUGUI oilPrice, oilProduction, oilDemand;
    public TextMeshProUGUI derricks, rigs, trucks, ships;

    public ProgressSlider environmentSlider, sentimentSlider, oilStorageSlider;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// Accepts a string
    public void NameDidChange(PubSubListenerEvent e) {
        buisnessName.SetText((string) e.value);
    }

    /// Accepts a DateTime
    public void DateDidChange(PubSubListenerEvent e) {
        DateTime newTime = (DateTime)e.value;
        string timeString = newTime.ToString("MMMM, yyyy");
        date.SetText(timeString);
    }

    /// Accepts a PrimaryInfoCardResourcesDollarsUpdate
    public void DollarsDidChange(PubSubListenerEvent e) {
        PrimaryInfoCardResourcesDollarsUpdate update = (PrimaryInfoCardResourcesDollarsUpdate)e.value;
        dollars.SetText(update.dollarsAmount.ToString("N0"));
        income.SetText(update.dollarsIncomePerSecond.ToString("N0"));
        expenses.SetText(update.dolarsExpensesPerSecond.ToString("N0"));
    }

    /// PrimaryInfoCardResourcesOilUpdate
    public void OilDidChange(PubSubListenerEvent e) {
        PrimaryInfoCardResourcesOilUpdate update = (PrimaryInfoCardResourcesOilUpdate)e.value;
        oilPrice.SetText(update.oilPrice.ToString("N2"));
        oilProduction.SetText(update.oilProducedPerSecond.ToString("N2"));
        oilDemand.SetText(update.oilConsumedPerSecond.ToString("N2"));
    }

    /// Accepts a PrimaryInfoCardResourcesEquipmentUpdate
    public void EquipmentDidChange(PubSubListenerEvent e) {

        PrimaryInfoCardResourcesEquipmentUpdate update = (PrimaryInfoCardResourcesEquipmentUpdate)e.value;
        derricks.SetText(update.numberOfDerricks.ToString("N0") + "/" + update.numberOfAllowedDerricks.ToString("N0"));
        rigs.SetText(update.numberOfRigs.ToString("N0") + "/" + update.numberOrAllowedRigs.ToString("N0"));
        trucks.SetText(update.numberOfTrucks.ToString("N0") + "/" + update.numberOfAllowedTrucks.ToString("N0"));
        ships.SetText(update.numberOfShips.ToString("N0") + "/" + update.numberOfAllowedShips.ToString("N0"));

    }

    // Accepts a double
    public void EvironmentalChange(PubSubListenerEvent e) {
        environmentSlider.progress = ((float) e.value);
    }

    // Accepts a double
    public void PublicSentimentDidChange(PubSubListenerEvent e) {
        sentimentSlider.progress = ((float) e.value);
    }

    // Accepts a double
    public void OilStorageDidChange(PubSubListenerEvent e) {
        oilStorageSlider.progress = ((float) e.value);
    }
}

public struct PrimaryInfoCardResourcesDollarsUpdate
{
    public decimal dollarsAmount, dollarsIncomePerSecond, dolarsExpensesPerSecond;
}

public struct PrimaryInfoCardResourcesOilUpdate
{
    public decimal oilPrice, oilProducedPerSecond, oilConsumedPerSecond;
}

public struct PrimaryInfoCardResourcesEquipmentUpdate
{
    public int numberOfDerricks, numberOfAllowedDerricks;
    public int numberOfRigs, numberOrAllowedRigs;
    public int numberOfTrucks, numberOfAllowedTrucks;
    public int numberOfShips, numberOfAllowedShips;
}