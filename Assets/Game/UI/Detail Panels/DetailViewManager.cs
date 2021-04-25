using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailViewManager : MonoBehaviour
{

    public CityDetailPanel cityDetailPanel;
    public GovernmentDetailPanel governmentDetailPanel;

    public OilSlickDetailPanel oilSlickDetailPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DidSelectCity(PubSubListenerEvent e) {
        City city = ((GameObject) e.value).gameObject.GetComponent<City>();
        cityDetailPanel.SetCity(city);
        cityDetailPanel.gameObject.SetActive(true);
    }

    public void DidDeselectCity(PubSubListenerEvent e) {
        City city = ((GameObject) e.value).gameObject.GetComponent<City>();
        if (cityDetailPanel.city.name == city.name) {
            cityDetailPanel.gameObject.SetActive(false);
        }
    }

    public void DidSelectGovernment(PubSubListenerEvent e) {
        Government government = ((GameObject) e.value).gameObject.GetComponent<Government>();
        governmentDetailPanel.SetGovernment(government);
        governmentDetailPanel.gameObject.SetActive(true);
    }

    public void DidDeselectGovernment(PubSubListenerEvent e) {
        Government government = ((GameObject) e.value).gameObject.GetComponent<Government>();
        if (governmentDetailPanel.government.name == government.name) {
            governmentDetailPanel.gameObject.SetActive(false);
        }
    }

    public void DidSelectOilSlick(PubSubListenerEvent e) {
        OilSlick oilSlick = ((GameObject) e.value).gameObject.GetComponent<OilSlick>();
        oilSlickDetailPanel.SetOilSlick(oilSlick);
        oilSlickDetailPanel.gameObject.SetActive(true);
    }

    public void DidDeselectOilSlick(PubSubListenerEvent e) {
        OilSlick oilSlick = ((GameObject) e.value).gameObject.GetComponent<OilSlick>();
        if (oilSlickDetailPanel.oilSlick == oilSlick) {
            oilSlickDetailPanel.gameObject.SetActive(false);
        }
    }
}
