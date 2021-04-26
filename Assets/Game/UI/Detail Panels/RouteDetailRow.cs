using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RouteDetailRow : MonoBehaviour
{

    public TextMeshProUGUI destination, length, amount;
    public Button add, subtract;
    public Image image;
    public Sprite truck, ship;
    public OilExtractor oilExtractor;
    public City destinationCity;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void refresh() {
        ResourceManager resourceManager = FindObjectOfType<ResourceManager>();
        
        destination.SetText(destinationCity.name);
        length.SetText(""); // TODO: Show how far one place is to the next...

        int connectionsAmount = 0;
        foreach (TradeRoute route in oilExtractor.TradeRoutes) {
            if ( route.City == destinationCity ) {
                connectionsAmount += 1;
            }
        }

        

        bool canAdd = true;
        if (oilExtractor.ExtractedOilType == OilSlickType.Land) {
            image.sprite = truck;
            amount.SetText(connectionsAmount.ToString() + " trucks");
            canAdd = resourceManager.CurrentOilTrucks < resourceManager.MaximumOilTrucks;
        } else {
            image.sprite = ship;
            amount.SetText(connectionsAmount.ToString() + " ships");
            canAdd = resourceManager.CurrentOilTankers < resourceManager.MaximumOilTankers;
        }

        add.interactable = canAdd;
        if (add.interactable) {
            add.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0,0,0, 0.4f);
        } else {
            add.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0,0,0, 0.1f);
        }

        subtract.interactable = connectionsAmount > 0;
        if (subtract.interactable) {
            subtract.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0,0,0, 0.4f);
        } else {
            subtract.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0,0,0, 0.1f);
        }
    }

    public void Setup(OilExtractor oilExtractor, City city) {
        this.oilExtractor = oilExtractor;
        this.destinationCity = city;
        refresh();
    }

    public void AddRoute() {
        oilExtractor.EstablishTradeRoute(destinationCity);
    }

    public void RemoveRoute() {
        oilExtractor.DestroyOneTradeRouteForCity(destinationCity);
    }
}
