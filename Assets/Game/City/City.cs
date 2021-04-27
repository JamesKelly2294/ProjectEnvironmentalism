using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class City : MonoBehaviour
{
    public string Name;
    public long Population;
    public Country Country;

    public float MaximumOilDemand = 1200; // units of oil
    public float CurrentOilDemand = 0; // units of oil

    [Range(0, 50)]
    public float OilDemandIncreaseRate = 10; // units per second

    [Range(0, 50)]
    public float MinimumOilIncreaseRate = 5; // units per second

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

    public GameObject DestroyedCityGraphic;
    public GameObject CityGraphic;

    public void DestroyCity(float destructionAnimationDuration)
    {
        StartCoroutine(AnimateCityDestruction(destructionAnimationDuration));
    }

    IEnumerator AnimateCityDestruction(float duration)
    {
        float elapsedTime = 0.0f;
        var destroyedCitySR = DestroyedCityGraphic.GetComponent<SpriteRenderer>();
        var citySR = CityGraphic.GetComponent<SpriteRenderer>();
        DestroyedCityGraphic.SetActive(true);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            destroyedCitySR.color = new Color(destroyedCitySR.color.r, destroyedCitySR.color.g, destroyedCitySR.color.b, (elapsedTime / duration));
            citySR.color = new Color(destroyedCitySR.color.r, destroyedCitySR.color.g, destroyedCitySR.color.b, 1 - (elapsedTime / duration));
            yield return new WaitForEndOfFrame();
        }

        CityGraphic.SetActive(false);
    }

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
        if (Country != Country.Hell)
        {
            UpdateOilDemand();
            if ((CurrentOilDemand / MaximumOilDemand) > 0.9)
            {
                sentiment = Mathf.Max(0, sentiment - (demandNotMetSentimentPenilty * Time.deltaTime));
            }
            else if ((CurrentOilDemand / MaximumOilDemand) < 0.1)
            {
                sentiment = Mathf.Min(1, sentiment + (demandNotMetSentimentPenilty * Time.deltaTime));
            }

            float demand = CurrentOilDemand / MaximumOilDemand;
            if (demand > 0.9)
            {
                float envimpact = Mathf.Pow(1.10f, numberOfTimesInvested) * 0.01f * Time.deltaTime;
                environment = Mathf.Clamp(environment + ((demand - 0.75f) * 4) * envimpact, 0, 1);
            }
        }

        OilDemandSlider.normalizedValue = CurrentOilDemand / MaximumOilDemand;
    }

    void UpdateOilDemand()
    {
        float oilIncreaseRate = MinimumOilIncreaseRate + ((OilDemandIncreaseRate - MinimumOilIncreaseRate) * sentiment);
        float oilIncrease = oilIncreaseRate * Time.deltaTime;
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
