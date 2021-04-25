using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PubSubSender))]
public class ResourceManager : MonoBehaviour
{
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
    
    public int CurrentMoney = 1_000_000;

    private PubSubSender _sender;

    void Awake()
    {
        _sender = GetComponent<PubSubSender>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PublishMoneyUpdate();
        PublishEquipmentUpdate();
        PublishEnvironmentHealthUpdate();
        PublishPublicSentimentUpdate();
        PublishOilStorageUpdate();
    }

    void PublishMoneyUpdate()
    {
        PrimaryInfoCardResourcesDollarsUpdate dollarsUpdate = new PrimaryInfoCardResourcesDollarsUpdate
        {
            dollarsAmount = CurrentMoney
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
        float environmentHealth = 0.5f;
        _sender.Publish("game.environment.health", environmentHealth);
    }

    void PublishPublicSentimentUpdate()
    {
        float publicSentiment = 0.5f;
        _sender.Publish("game.sentiment.value", publicSentiment);
    }

    void PublishOilStorageUpdate()
    {
        float oilStorage = 0.5f;
        _sender.Publish("game.oil.storage.usage", oilStorage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
