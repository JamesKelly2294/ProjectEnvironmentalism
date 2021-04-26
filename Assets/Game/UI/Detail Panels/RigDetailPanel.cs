using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RigDetailPanel : MonoBehaviour
{
    public TextMeshProUGUI extraction;
    public ProgressSlider oilReserves, oilStorage;
    public OilExtractor oilExtractor;
    public GameObject productionTabButton, routesTabButton;
    public GameObject productionTab, routesTab;
    public GameObject routeRowPrefab, countryRowPrefab;
    public GameObject routesList;
    public TextMeshProUGUI routesUsedScreen;

    // Start is called before the first frame update
    void Start()
    {
        SwitchToProductionTab();
    }

    // Update is called once per frame
    void Update()
    {
        if (oilExtractor == null) {
            return;
        }

        extraction.SetText("Extracts " + oilExtractor.OilExtractionRate.ToString() + "/s");
        oilReserves.progress = oilExtractor.CurrentOilReserves / oilExtractor.MaxOilReserves;
        oilStorage.progress = oilExtractor.CurrentOilStorage / oilExtractor.MaxOilStorage;
    }

    public void routesDidChange() {
        ResourceManager rm = GameObject.FindObjectOfType<ResourceManager>();
        int tankers = rm.MaximumOilTankers - rm.CurrentOilTankers;
        if (tankers > 1) {
            routesUsedScreen.SetText(tankers + " tankers are available");
        } else if ( tankers == 1 ) {
            routesUsedScreen.SetText("One tanker is available");
        } else {
            routesUsedScreen.SetText("No tankers are available");
        }
        
    }

    public void refresh() {
        // Remove the current routes
        foreach (Transform child in routesList.transform) {
            Destroy(child.gameObject);
        }

        ResourceManager rm = GameObject.FindObjectOfType<ResourceManager>();
        List<City> sortedCities = rm.Cities.OrderBy(c=>c.Name).ToList();

        // Add in Cuban cities
        GameObject cuba = Instantiate(countryRowPrefab, routesList.transform);
        cuba.GetComponent<CountryDetailRow>().Setup(Country.Cuba);
        foreach (City city in sortedCities) {
            if (city.Country == Country.Cuba) {
                GameObject routeRow = Instantiate(routeRowPrefab, routesList.transform);
                routeRow.GetComponent<RouteDetailRow>().Setup(oilExtractor, city);
            }
        }

        // Add in Mexican cities
        GameObject mexico = Instantiate(countryRowPrefab, routesList.transform);
        mexico.GetComponent<CountryDetailRow>().Setup(Country.Mexico);
        foreach (City city in sortedCities) {
            if (city.Country == Country.Mexico) {
                GameObject routeRow = Instantiate(routeRowPrefab, routesList.transform);
                routeRow.GetComponent<RouteDetailRow>().Setup(oilExtractor, city);
            }
        }
        
        // Add in American cities
        GameObject us = Instantiate(countryRowPrefab, routesList.transform);
        us.GetComponent<CountryDetailRow>().Setup(Country.UnitedStates);
        foreach (City city in sortedCities) {
            if (city.Country == Country.UnitedStates) {
                GameObject routeRow = Instantiate(routeRowPrefab, routesList.transform);
                routeRow.GetComponent<RouteDetailRow>().Setup(oilExtractor, city);
            }
        }

        routesDidChange();
    }

    public void SetOilExtractor(OilExtractor extractor) {
        this.oilExtractor = extractor;
        refresh();
    }

    public void SwitchToProductionTab() {
        productionTab.SetActive(true);
        routesTab.SetActive(false);
        productionTabButton.GetComponent<Image>().enabled = true;
        routesTabButton.GetComponent<Image>().enabled = false;
    }

    public void SwitchToRoutesTab() {
        productionTab.SetActive(false);
        routesTab.SetActive(true);
        productionTabButton.GetComponent<Image>().enabled = false;
        routesTabButton.GetComponent<Image>().enabled = true;
    }
}
