using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PubSubSender))]
[RequireComponent(typeof(PubSubListener))]
public class ResourceManager : MonoBehaviour
{
    public GameObject OilExtractorPrefab;

    public OilSlick StartingOilSlick;

    [Range(0, 10)]
    public int MaximumOilDerricks;
    public int CurrentOilDerricks;

    [Range(0, 10)]
    public int MaximumOilRigs;
    public int CurrentOilRigs;

    [Range(0, 10)]
    public int MaximumOilTrucks;
    public int CurrentOilTrucks;

    [Range(0, 10)]
    public int MaximumOilTankers;
    public int CurrentOilTankers;
    
    public float CurrentMoney = 1_000_000;
    public decimal CurrentOilPrice = new decimal(97.05);

    [Range(0, 1)]

    public float BaseGreenProgressRate = 0.5f;
    [Range(0, 1)]
    public float BasePublicSentimentDecayRate = 0.5f;

    private PubSubSender _sender;

    [Range(0, 100)]
    public float EnvironmentHealth = 50f;
    [Range(0, 100)]
    public float PublicSentiment = 30f;
    [Range(0, 1.0f)]
    public float OilStorage = 0.5f;

    private List<OilExtractor> oilExtractors = new List<OilExtractor>();
    private List<OilSlick> purchasedOilSlicks = new List<OilSlick>();

    public List<City> Cities = new List<City>();

    void Awake()
    {
        _sender = GetComponent<PubSubSender>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PublishOilUpdate();
        PublishMoneyUpdate();
        PublishEquipmentUpdate();
        PublishEnvironmentHealthUpdate();
        PublishPublicSentimentUpdate();
        PublishOilStorageUpdate();

        PurchaseStartingOilSlick();
    }

    void PurchaseStartingOilSlick()
    {
        PurchaseOilSlick(StartingOilSlick);
    }

    void CalculateOilMetrics()
    {
        PublishOilUpdate();
    }

    void CalculateMoney()
    {
        PublishMoneyUpdate();
    }

    void CalculateEnvironmentHealth()
    {
        // The world marches ever onwards towards green energy
        EnvironmentHealth += (BaseGreenProgressRate * Time.deltaTime);
        EnvironmentHealth = Mathf.Clamp(EnvironmentHealth, 0.0f, 100.0f);
        PublishEnvironmentHealthUpdate();
    }

    void CalculatePublicSentiment()
    {
        // Public sentiment trends towards the center
        if (PublicSentiment < (50.0f - 0.001f))
        {
            PublicSentiment += BasePublicSentimentDecayRate * Time.deltaTime;
            PublicSentiment = Mathf.Min(PublicSentiment, 50.0f);
        }
        else if (PublicSentiment > (50.0f + 0.001f))
        {
            PublicSentiment -= BasePublicSentimentDecayRate * Time.deltaTime;
            PublicSentiment = Mathf.Max(PublicSentiment, 50.0f);
        }
        else
        {
            PublicSentiment = 50.0f;
        }
        PublicSentiment = Mathf.Clamp(PublicSentiment, 0.0f, 100.0f);
        PublishPublicSentimentUpdate();
    }

    void CalculateOilStorage()
    {
        float totalOilStorageCapacity = 0;
        float totalOilStored = 0;

        oilExtractors.ForEach(e =>
        {
            totalOilStorageCapacity += e.MaxOilStorage;
            totalOilStored += e.CurrentOilStorage;
        });

        OilStorage = totalOilStorageCapacity > 0 ? totalOilStored / totalOilStorageCapacity: 1.0f;

        PublishOilStorageUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateOilMetrics();
        CalculateMoney();
        CalculateEnvironmentHealth();
        CalculatePublicSentiment();
        CalculateOilStorage();
    }

    void PublishOilUpdate()
    {
        PrimaryInfoCardResourcesOilUpdate dollarsUpdate = new PrimaryInfoCardResourcesOilUpdate
        {
            oilPrice = CurrentOilPrice
        };

        _sender.Publish("game.oil.totals", dollarsUpdate);
    }
    
    void PublishMoneyUpdate()
    {
        PrimaryInfoCardResourcesDollarsUpdate dollarsUpdate = new PrimaryInfoCardResourcesDollarsUpdate
        {
            dollarsAmount = Mathf.RoundToInt(CurrentMoney)
        };

        _sender.Publish("game.dollars.totals", dollarsUpdate);
    }

    void PublishEquipmentUpdate()
    {
        PrimaryInfoCardResourcesEquipmentUpdate equipmentUpdate = new PrimaryInfoCardResourcesEquipmentUpdate
        {
            numberOfAllowedDerricks = MaximumOilDerricks,
            numberOfDerricks = CurrentOilDerricks,
            numberOrAllowedRigs = MaximumOilRigs,
            numberOfRigs = CurrentOilRigs,
            numberOfAllowedTrucks = MaximumOilTrucks,
            numberOfTrucks = CurrentOilTrucks,
            numberOfAllowedShips = MaximumOilTankers,
            numberOfShips = CurrentOilTankers
        };
        _sender.Publish("game.equipment.totals", equipmentUpdate);
    }

    void PublishEnvironmentHealthUpdate()
    {
        _sender.Publish("game.environment.health", EnvironmentHealth / 100.0f);
    }

    void PublishPublicSentimentUpdate()
    {
        _sender.Publish("game.sentiment.value", PublicSentiment / 100.0f);
    }

    void PublishOilStorageUpdate()
    {
        _sender.Publish("game.oil.storage.usage", OilStorage);
    }

    public void RegisterOilExtractor(OilExtractor oilExtractor)
    {
        if(oilExtractors.Contains(oilExtractor))
        {
            return;
        }
        oilExtractors.Add(oilExtractor);
    }

    public void UnregisterOilExtractor(OilExtractor oilExtractor)
    {
        oilExtractors.Remove(oilExtractor);
    }

    public void RegisterCity(City city)
    {
        if (Cities.Contains(city))
        {
            return;
        }
        Cities.Add(city);
    }

    public void UnregisterCity(City city)
    {
        Cities.Remove(city);
    }

    public bool AttemptToReserveVehicle(OilSlickType type) { 
        if (type == OilSlickType.Land)
        {
            if (CurrentOilTrucks < MaximumOilTrucks)
            {
                CurrentOilTrucks += 1;
                PublishEquipmentUpdate();
                AudioManager.Instance.Play("Resource/Purchase/OilTruck");
                return true;
            }
            else
            {
                AudioManager.Instance.Play("Resource/Error");
                return false;
            }
        }
        else
        {
            if (CurrentOilTankers < MaximumOilTankers)
            {
                CurrentOilTankers += 1;
                PublishEquipmentUpdate();
                AudioManager.Instance.Play("Resource/Purchase/OilTanker");
                return true;
            }
            else
            {
                AudioManager.Instance.Play("Resource/Error");
                return false;
            }
        }
    }

    public void ReturnVehicle(OilSlickType type) {
        if (type == OilSlickType.Land) {
            CurrentOilTrucks -= 1;
        } else {
            CurrentOilTankers -= 1;
        }
        PublishEquipmentUpdate();
    }

    public bool AttemptPurchase(float amount)
    {
        if(amount < 0)
        {
            return false;
        }
        else if (amount > CurrentMoney)
        {
            return false;
        }
        else
        {
            CurrentMoney -= amount;
            return true;
        }
    }

    public void PurchaseOilSlick(OilSlick oilSlick)
    {
        if(!oilSlick)
        {
            return;
        }

        Debug.Log("Purchase oil slick: " + oilSlick);
        float oilSlickCost = purchasedOilSlicks.Count == 0 ? 0 : oilSlick.CostToPurchase;
        if (AttemptPurchase(oilSlickCost))
        {
            var oilExtractor = Instantiate(OilExtractorPrefab);
            oilExtractor.transform.position = oilSlick.transform.position;
            oilExtractor.GetComponent<OilExtractor>().SetOilSlick(oilSlick);
            purchasedOilSlicks.Add(oilSlick);

            if (oilSlick.type == OilSlickType.Land) {
                CurrentOilDerricks += 1;
            } else {
                CurrentOilRigs += 1;
            }

            if (oilSlickCost != 0) {
                AudioManager.Instance.Play("Resource/KaChing");
            }

            oilExtractor.GetComponent<SelectableSprite>().TriggerBoxSelect();
            PublishEquipmentUpdate();
        } 
    }

    public void PurchaseOilSlickEvent(PubSubListenerEvent e)
    {
        var oilSlick = e.value as OilSlick;
        if (oilSlick)
        {
            PurchaseOilSlick(oilSlick);
        }
    }
}
