using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CityDetailPanel : MonoBehaviour
{

    public TextMeshProUGUI title;
    public TextMeshProUGUI subtitle;

    public ProgressSlider environment, sentiment, demand;

    public Image flag;

    public City city;

    public Sprite flagUS, flagCuba, flagMexico, flagHell;


    public Button bribeButton, investButton;
    public TextMeshProUGUI bribeButtonText, investButtonText;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (city == null) {
            return;
        }

        environment.progress = city.environment;
        demand.progress = city.CurrentOilDemand / city.MaximumOilDemand;
        sentiment.progress = city.sentiment;


        ResourceManager rm = GameObject.FindObjectOfType<ResourceManager>();
        bribeButton.interactable = rm.CurrentMoney >= city.bribeCost;
        bribeButtonText.SetText("Bribe ($" + city.bribeCost.ToString("N2") + ")");
        
        investButton.interactable = (rm.CurrentMoney >= city.investCost) && (city.sentiment >= city.requiredInvestSentiment);
        string investButtonString = "Invest ($" + city.investCost.ToString("N2") + ")";
        if (city.sentiment < city.requiredInvestSentiment) {
            investButtonString += " (Sentiment must be >= " + (city.requiredInvestSentiment * 100).ToString("N0") + "%)";
        }
        investButtonText.SetText(investButtonString);
    }

    void refresh() {
        subtitle.SetText(city.Population.ToString("N0") + " souls");

        if (city.Country == Country.UnitedStates) {
            flag.sprite = flagUS;
            title.SetText(city.Name + ", U.S.");
        } else if (city.Country == Country.Cuba) {
            flag.sprite = flagCuba;
            title.SetText(city.Name + ", Cuba");
        } else if (city.Country == Country.Mexico) {
            flag.sprite = flagMexico;
            title.SetText(city.Name + ", Mexico");
        } else if (city.Country == Country.Hell)
        {
            flag.sprite = flagHell;
            title.SetText(city.Name);
        }

    }

    public void SetCity(City city) {
        this.city = city;
        refresh();
    }

    public void Bribe() {
        city.Bribe();
    }

    public void Invest() {
        city.Invest();
    }
}
