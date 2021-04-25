using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailViewManager : MonoBehaviour
{

    public CityDetailPanel cityDetailPanel;

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
}
