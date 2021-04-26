using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeRoute : MonoBehaviour
{
    public City City;
    public OilExtractor OilExtractor;
    public GameObject OilTruck;
    public GameObject OilTanker;

    public GameObject OilVehicle { get; private set; }

    private RoadManager _roadManager;

    // Start is called before the first frame update
    void Start()
    {
        if (OilExtractor.ExtractedOilSlick.type == OilSlickType.Land)
        {
            OilVehicle = OilTruck;
        }
        else
        {
            OilVehicle = OilTanker;
        }
        OilVehicle.SetActive(true);
        OilVehicle.transform.position = OilExtractor.DockingArea.transform.position;

        City.RegisterTradeRoute(this);
        OilExtractor.RegisterTradeRoute(this);

        _roadManager = FindObjectOfType<RoadManager>();
        _roadManager.GenerateTradeRoutePath(this);
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
