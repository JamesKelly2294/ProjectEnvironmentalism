using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class City : MonoBehaviour
{
    public string Name;
    [Range(0, 10_000_000)]
    public int Population;
    public Country Country;

    public float MaximumOilDemand = 1200; // units of oil
    public float CurrentOilDemand = 10; // units of oil

    [Range(0, 50)]
    public float OilDemandIncreaseRate = 10; // units per second

    public Slider OilDemandSlider;

    public TMPro.TextMeshPro Label;

    private ResourceManager _resourceManager;

    public List<TradeRoute> TradeRoutes = new List<TradeRoute>();

    public float sentiment = 0.5f;
    public float demandMetSentimentBonus = 0.01f, demandNotMetSentimentPenilty = 0.05f;
    public float bribeCost = 100;
    public float investCost = 10_000;
    public int numberOfTimesInvested = 0;
    public float requiredInvestSentiment = 0.5f;

    public float environment = 0.9f;
    public float pollutionPerMillionOil = 10f;

    // Start is called before the first frame update
    void Start()
    {
        Label.text = Name;
        _resourceManager = FindObjectOfType<ResourceManager>();
        _resourceManager.RegisterCity(this);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateOilDemand();
        OilDemandSlider.normalizedValue = CurrentOilDemand / MaximumOilDemand;
        if ((CurrentOilDemand / MaximumOilDemand) > 0.9) {
            sentiment = Mathf.Max(0, sentiment - (demandNotMetSentimentPenilty * Time.deltaTime ));
        } else if ((CurrentOilDemand / MaximumOilDemand) < 0.1) {
            sentiment = Mathf.Min(1, sentiment + (demandNotMetSentimentPenilty * Time.deltaTime ));
        }

        float demand = CurrentOilDemand / MaximumOilDemand;
        if (demand > 0.75) {
            float envimpact = Mathf.Pow(1.10f, numberOfTimesInvested) * 0.02f * Time.deltaTime ;
            environment = Mathf.Clamp(environment + ((demand - 0.75f) * 4) * envimpact, 0, 1);
        }
    }

    void UpdateOilDemand()
    {
        CurrentOilDemand = Mathf.Min(MaximumOilDemand, CurrentOilDemand + OilDemandIncreaseRate * Time.deltaTime);
    }

    private void OnDestroy()
    {
        _resourceManager.UnregisterCity(this);
    }

    public void RegisterTradeRoute(TradeRoute tradeRoute)
    {
        if (TradeRoutes.Contains(tradeRoute))
        {
            return;
        }
        TradeRoutes.Add(tradeRoute);
    }

    public void UnregisterTradeRoute(TradeRoute tradeRoute)
    {
        TradeRoutes.Remove(tradeRoute);
    }

    public void Bribe() {
        ResourceManager rm = GameObject.FindObjectOfType<ResourceManager>();

        if (rm.AttemptPurchase(bribeCost)) {
            bribeCost *= 2;
            sentiment = 1;
        }
    }

    public void Invest() {
        ResourceManager rm = GameObject.FindObjectOfType<ResourceManager>();

        if (sentiment < requiredInvestSentiment) {
            return;
        }

        if (rm.AttemptPurchase(investCost)) {
            investCost *= 2;
            OilDemandIncreaseRate *= 2;
            numberOfTimesInvested += 1;
        }
    }

    public void SellOil(float amount) {
        ResourceManager rm = GameObject.FindObjectOfType<ResourceManager>();
        rm.SellOil(amount);
        environment = Mathf.Clamp(environment - (pollutionPerMillionOil * (amount / 1_000_000.0f)), 0, 1);
        CurrentOilDemand -= amount;
    }
}
