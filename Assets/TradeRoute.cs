using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeRoute : MonoBehaviour
{
    public City City;
    public OilExtractor OilExtractor;
    public GameObject OilTruck;
    public GameObject OilTanker;

    private GameObject _oilVehicle;

    // Start is called before the first frame update
    void Start()
    {
        if (OilExtractor.ExtractedOilSlick.type == OilSlickType.Land)
        {
            _oilVehicle = OilTruck;
        }
        else
        {
            _oilVehicle = OilTanker;
        }
        _oilVehicle.SetActive(true);

        City.RegisterTradeRoute(this);
        OilExtractor.RegisterTradeRoute(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        City.UnregisterTradeRoute(this);
        OilExtractor.UnregisterTradeRoute(this);
    }
}
