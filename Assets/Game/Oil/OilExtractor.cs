using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OilExtractor : MonoBehaviour
{
    public GameObject TradeRoutePrefab;

    public OilSlick ExtractedOilSlick { get; protected set; }

    public GameObject oilDerrickGraphic;
    public GameObject oilRigGraphic;

    public Slider OilReservesSlider;
    public Slider OilStorageSlider;

    public Transform DockingArea;

    public float MaxOilReserves = 1200;
    public float CurrentOilReserves;
    public float MaxOilStorage = 100;
    public float CurrentOilStorage = 0;
    [Range(0, 50)]
    public float OilExtractionRate = 10; // units per second

    public OilSlickType ExtractedOilType { get { return ExtractedOilSlick.type; } }

    public List<TradeRoute> TradeRoutes = new List<TradeRoute>();

    private ResourceManager _resourceManager;
    private PubSubSender _sender;

    public float digDeeperCost = 100;

    // Start is called before the first frame update
    void Start()
    {
        CurrentOilReserves = MaxOilReserves;
        _sender = GetComponent<PubSubSender>();

        _resourceManager = GameObject.FindObjectOfType<ResourceManager>();
        _resourceManager.RegisterOilExtractor(this);
    }

    public void SetOilSlick(OilSlick incomingOilSlick)
    {
        if(ExtractedOilSlick != null)
        {
            return;
        }

        ExtractedOilSlick = incomingOilSlick;
        incomingOilSlick.transform.parent = transform;
        ExtractedOilSlick.gameObject.SetActive(false);

        if(ExtractedOilSlick.type == OilSlickType.Land)
        {
            oilRigGraphic.SetActive(false);
            oilDerrickGraphic.SetActive(true);
            gameObject.GetComponent<SelectableSprite>().SelectionType = "oilderrick";
        }
        else
        {
            oilRigGraphic.SetActive(true);
            oilDerrickGraphic.SetActive(false);
            gameObject.GetComponent<SelectableSprite>().SelectionType = "oilrig";
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateOilExtraction();
        OilReservesSlider.normalizedValue = CurrentOilReserves / MaxOilReserves;
        OilStorageSlider.normalizedValue = CurrentOilStorage / MaxOilStorage;
    }

    void UpdateOilExtraction()
    {
        float prevCurrentOilReserves = CurrentOilReserves;
        float prevCurrentOilStorage = CurrentOilStorage;

        float extractionRate = OilExtractionRate * Time.deltaTime;
        float extractableOil = CurrentOilReserves < extractionRate ? CurrentOilReserves : extractionRate;
        float storableOil = MaxOilStorage - CurrentOilStorage;
        float extractedOil = storableOil < extractableOil ? storableOil : extractableOil;
        CurrentOilReserves -= extractedOil;
        CurrentOilStorage += extractedOil;

        if (prevCurrentOilReserves > 0 && CurrentOilReserves <= 0) {
            EventManager em = GameObject.FindObjectOfType<EventManager>();
            em.SummonDryExtractorNotification(this);
        }
    }

    public void EstablishTradeRoute(City city)
    {
        if (city == null) { return; }
        if (false == _resourceManager.AttemptToReserveVehicle(ExtractedOilSlick.type)) { return; }
        var go = Instantiate(TradeRoutePrefab);
        go.transform.position = transform.position;
        go.transform.parent = transform;
        go.transform.name = city.Name + " Trade Route";

        TradeRoute tradeRoute = go.GetComponent<TradeRoute>();
        tradeRoute.City = city;
        tradeRoute.OilExtractor = this;
    }

    public void DestroyOneTradeRouteForCity(City city) {
        foreach(TradeRoute tradeRoute in GetComponentsInChildren<TradeRoute>()) {
            if (tradeRoute.City == city) {
                Destroy(tradeRoute.gameObject);
                return;
            }
        }
    }

    private void OnDestroy()
    {
        _resourceManager.UnregisterOilExtractor(this);
    }

    public void RegisterTradeRoute(TradeRoute tradeRoute)
    {
        if (TradeRoutes.Contains(tradeRoute))
        {
            return;
        }
        TradeRoutes.Add(tradeRoute);
        _sender.Publish("oilextractor.traderoute.changed", this);
    }

    public void UnregisterTradeRoute(TradeRoute tradeRoute)
    {
        if (!TradeRoutes.Contains(tradeRoute)) {
            return;
        }

        TradeRoutes.Remove(tradeRoute);
        _resourceManager.ReturnVehicle(ExtractedOilSlick.type);

        _sender.Publish("oilextractor.traderoute.changed", this);
    }

    public void DigDeeper() {
        ResourceManager rm = GameObject.FindObjectOfType<ResourceManager>();
        if (rm.AttemptPurchase(digDeeperCost)) {
            digDeeperCost *= 2;
            CurrentOilReserves += MaxOilReserves;
            MaxOilReserves *= 2;
        }
    }
}
