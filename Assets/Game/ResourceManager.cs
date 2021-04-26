using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningAverage
{
    private float[] dataBuffer = new float[60];
    private int dataPointer = 0;
    private int dataBufferSize = 0;

    private float calculatedAverage = 0.0f;
    private bool dirty = false;

    public void InsertNewValue(float value)
    {
        dataBuffer[dataPointer] = value;

        dataBufferSize = Math.Min(dataBufferSize + 1, 60);

        dataPointer = (dataPointer + 1) % 60;

        dirty = true;
    }

    public float CalculatedAverage()
    {
        if(dirty)
        {
            float runningIncomeRate = 0.0f;
            for (int i = 0; i < dataBufferSize; i++)
            {
                runningIncomeRate += dataBuffer[i];
            }
            runningIncomeRate /= dataBufferSize;
            calculatedAverage = runningIncomeRate;
            dirty = false;
        }

        return calculatedAverage;
    }
}

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

    public float BaseCostDerrick = 1_000;
    public float BaseCostRig = 5_000;

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
    
    private float elapsedTime = 0.0f;

    RunningAverage incomeRunningAvg = new RunningAverage();
    RunningAverage expensesRunningAvg = new RunningAverage();

    RunningAverage oilProductionRunningAvg = new RunningAverage();
    RunningAverage oilExportRunningAvg = new RunningAverage();

    private float _depositedFunds = 0.0f;
    private float _withdrawnFunds = 0.0f;
    private float _producedOil = 0.0f;
    private float _exportedOil = 0.0f;



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
        
        float env = 0;
        foreach (City city in Cities) {
            env += city.environment * 100.0f;
        }
        EnvironmentHealth = env / Cities.Count;

        PublishEnvironmentHealthUpdate();
    }

    void CalculatePublicSentiment()
    {
        float sentiment = 0;
        foreach (City city in Cities) {
            sentiment += city.sentiment * 100.0f;
        }
        PublicSentiment = sentiment / Cities.Count;

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
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 1.0f)
        {
            elapsedTime = 0.0f;

            incomeRunningAvg.InsertNewValue(_depositedFunds);
            expensesRunningAvg.InsertNewValue(_withdrawnFunds);

            oilProductionRunningAvg.InsertNewValue(_producedOil);
            oilExportRunningAvg.InsertNewValue(_exportedOil);

            _depositedFunds = 0.0f;
            _withdrawnFunds = 0.0f;

            _producedOil = 0.0f;
            _exportedOil = 0.0f;
        }

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
            oilPrice = CurrentOilPrice,
            oilProducedPerSecond = (decimal)oilProductionRunningAvg.CalculatedAverage(),
            oilConsumedPerSecond = (decimal)oilExportRunningAvg.CalculatedAverage(),
        };

        _sender.Publish("game.oil.totals", dollarsUpdate);
    }
    
    void PublishMoneyUpdate()
    {
        PrimaryInfoCardResourcesDollarsUpdate dollarsUpdate = new PrimaryInfoCardResourcesDollarsUpdate
        {
            dollarsAmount = Mathf.RoundToInt(CurrentMoney),
            dollarsIncomePerSecond = (decimal)incomeRunningAvg.CalculatedAverage(),
            dolarsExpensesPerSecond = (decimal)expensesRunningAvg.CalculatedAverage(),
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
                AudioManager.Instance.Play("Resource/Purchase/OilTruck", pitchMin: 0.95f, pitchMax: 1.05f);
                return true;
            }
            else
            {
                AudioManager.Instance.Play("Resource/Error", pitchMin: 0.8f, pitchMax: 1.2f);
                return false;
            }
        }
        else
        {
            if (CurrentOilTankers < MaximumOilTankers)
            {
                CurrentOilTankers += 1;
                PublishEquipmentUpdate();
                AudioManager.Instance.Play("Resource/Purchase/OilTanker", pitchMin: 0.95f, pitchMax: 1.05f);
                return true;
            }
            else
            {
                AudioManager.Instance.Play("Resource/Error", pitchMin: 0.8f, pitchMax: 1.2f);
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

    public bool DepositFunds(float amount, bool playSoundEffect = true)
    {
        if (playSoundEffect)
        {
            AudioManager.Instance.Play("Resource/KaChing", pitchMin: 0.8f, pitchMax: 1.2f, volumeMin: 0.45f, volumeMax: 0.65f);
        }
        CurrentMoney += amount;
        _depositedFunds += amount;
        return true;
    }

    public bool AttemptPurchase(float amount)
    {
        if(amount < 0)
        {
            AudioManager.Instance.Play("Resource/Error", pitchMin: 0.8f, pitchMax: 1.2f);
            return false;
        }
        else if (amount > CurrentMoney)
        {
            AudioManager.Instance.Play("Resource/Error", pitchMin: 0.8f, pitchMax: 1.2f);
            return false;
        }
        else
        {
            CurrentMoney -= amount;
            _withdrawnFunds += amount;
            if (amount != 0)
            {
                AudioManager.Instance.Play("Resource/KaChing", pitchMin: 0.8f, pitchMax: 1.2f);
            }
            return true;
        }
    }

    public void SellOil(float amount) {
        DepositFunds((float)CurrentOilPrice * amount, playSoundEffect: false);
        _exportedOil += amount;
    }
    
    public void OilProduced(float oilQuantity)
    {
        _producedOil += oilQuantity;
    }

    public void PurchaseOilSlick(OilSlick oilSlick)
    {
        if(!oilSlick)
        {
            return;
        }

        Debug.Log("Purchase oil slick: " + oilSlick);
        (bool, bool, float) affordability = PriceToBuyOilSlick(oilSlick);
        float oilSlickCost = affordability.Item3;
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

            oilExtractor.GetComponent<SelectableSprite>().TriggerBoxSelect();
            PublishEquipmentUpdate();
        } 
    }

    public (bool, bool, float) PriceToBuyOilSlick(OilSlick oilSlick) {

        float money = CurrentMoney;
        bool allowedToBuy = true;
        float cost = 0;

        if (oilSlick.type == OilSlickType.Land) {
            allowedToBuy = CurrentOilDerricks < MaximumOilDerricks;
            cost = BaseCostDerrick * (float)Math.Pow(1.1, CurrentOilDerricks);

            if (CurrentOilDerricks == 0) {
                cost = 0;
                allowedToBuy = true;
            }

        } else {
            allowedToBuy = CurrentOilRigs < MaximumOilRigs;

            int level = 1;
            if (oilSlick.level == OilSlickLevel.Sea2) {
                level = 2;
            } else if (oilSlick.level == OilSlickLevel.Sea3) {
                level = 3;
            } else if (oilSlick.level == OilSlickLevel.Sea4) {
                level = 4;
            }

            cost = BaseCostRig * (float)Math.Pow(1.1, CurrentOilRigs) * (float)Math.Pow(3.5, level);
        }

        return (allowedToBuy, money >= cost, cost);
    }

    public float PriceToBuyOilDerrick() {
        return 100_000f * (float)Math.Pow(1.06, MaximumOilDerricks);
    }

    public float PriceToBuyOilRig() {
        return 10_000_000f * (float)Math.Pow(1.06, MaximumOilRigs);
    }

    public float PriceToBuyOilTruck() {
        return 14_000f * (float)Math.Pow(1.06, MaximumOilTrucks);
    }

    public float PriceToBuyOilTanker() {
        return 1_000_000f * (float)Math.Pow(1.06, MaximumOilTankers);
    }

    public void PurchaseOilSlickEvent(PubSubListenerEvent e)
    {
        var oilSlick = e.value as OilSlick;
        if (oilSlick)
        {
            PurchaseOilSlick(oilSlick);
        }
    }

    public void BuyDerrick() {
        float priceForDerrick = PriceToBuyOilDerrick();

        if(AttemptPurchase(priceForDerrick)) {
            MaximumOilDerricks += 1;
            PublishEquipmentUpdate();
        }
    }

    public void BuyRig() {
        float priceForRig = PriceToBuyOilRig();

        if(AttemptPurchase(priceForRig)) {
            MaximumOilRigs += 1;
            PublishEquipmentUpdate();
        }
    }

    public void BuyTruck() {
        float priceForTruck = PriceToBuyOilTruck();

        if(AttemptPurchase(priceForTruck)) {
            MaximumOilTrucks += 1;
            PublishEquipmentUpdate();
        }
    }

    public void BuyTanker() {
        float priceForTanker = PriceToBuyOilTanker();

        if(AttemptPurchase(priceForTanker)) {
            MaximumOilTankers += 1;
            PublishEquipmentUpdate();
        }
    }
}
