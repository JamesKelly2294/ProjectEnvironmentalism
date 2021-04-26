using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Country
{
    UnitedStates,
    Mexico,
    Cuba,
    Hell
}

public class Government : MonoBehaviour
{
    public string Name;
    public int Population;
    public Country Country;

    public float environment = 1, sentiment = 0.5f;
    
    public TMPro.TextMeshPro Label;

    public bool hasGrantedExpandedOilRights = false;

    // Start is called before the first frame update
    void Start()
    {
        Label.text = Name;
    }

    // Update is called once per frame
    void Update()
    {

        int cities = 0;
        float allEnvironment = 0;
        float allSentiment = 0;
        ResourceManager rm = GameObject.FindObjectOfType<ResourceManager>();
        foreach( City city in rm.Cities ) {
            if (city.Country == Country) {
                cities += 1;
                allEnvironment += city.environment;
                allSentiment += city.sentiment;
            }
        }
        allEnvironment /= cities;
        allSentiment /= cities;

        sentiment = (sentiment * 0.9f) + (allSentiment * 0.1f);
        environment = (environment * 0.5f) + (allEnvironment * 0.5f);
    }

    public void ExpandOilRights () {
        ResourceManager rm = GameObject.FindObjectOfType<ResourceManager>();
        OilSlickManager osm = GameObject.FindObjectOfType<OilSlickManager>();
        (bool, float) affordability = osm.PriceToUnlockNextOilSlick();

        if (rm.AttemptPurchase(affordability.Item2)) {
            hasGrantedExpandedOilRights = true;
            osm.UnlockOilSlickLevel(osm.CurrentOilSlickLevel + 1);
        }
    }
}
