using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryInfoCard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public struct PrimaryInfoCardResourcesUpdate
{
    public decimal dollarsAmount, dollarsIncomePerSecond, dolarsExpensesPerSecond;
    public decimal oilPrice, oilProduced, oilConsumed;
    public int numberOfDerricks, numberOfAllowedDerricks;
    public int numberOfRigs, numberOrAllowedRigs;
    public int numberOfTrucks, numberOfAllowedTrucks;
    public int numberOfShips, numberOfAllowedShips;
    public double environmentalHealth;
    public double publicSentiment;

}