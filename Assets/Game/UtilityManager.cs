using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UtilityManager : MonoBehaviour
{

    public float power = 60f;
    public float water = 60f;
    public float powerThreashold = 50f;
    public float waterThreashold = 50f;

    public float powerGeneration = 10f;
    public float waterGeneration = 10f;
    public float powerUsage = 11f;
    public float waterUsage = 8f;

    public TextMeshProUGUI powerTMP, waterTMP;

    public UtilityState powerState = UtilityState.ok;
    public UtilityState waterState = UtilityState.ok;

    public float powerTimeAboveThreashoad = 1f;
    public float waterTimeAboveThreashold = 1f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        power += (powerGeneration - powerUsage) * Time.deltaTime;
        water += (waterGeneration - waterUsage) * Time.deltaTime;
        power = Mathf.Min(100f, Mathf.Max(0f, power));
        water = Mathf.Min(100f, Mathf.Max(0f, water));

        if (power >= powerThreashold) {
            powerTimeAboveThreashoad += Time.deltaTime;
        } else {
            powerTimeAboveThreashoad -= Time.deltaTime;
        }
        powerTimeAboveThreashoad = Mathf.Min(1f, Mathf.Max(0f, powerTimeAboveThreashoad));

        if (water >= waterThreashold) {
            waterTimeAboveThreashold += Time.deltaTime;
        } else {
            waterTimeAboveThreashold -= Time.deltaTime;
        }
         waterTimeAboveThreashold = Mathf.Min(1f, Mathf.Max(0f, waterTimeAboveThreashold));

        if (powerState == UtilityState.low && powerTimeAboveThreashoad >= 1f) {
            powerState = UtilityState.ok;
            GetComponent<PubSubSender>().Publish("power.ok");
        } else if (powerState == UtilityState.ok && powerTimeAboveThreashoad <= 0f) {
            powerState = UtilityState.low;
            GetComponent<PubSubSender>().Publish("power.low");
        }

        if (waterState == UtilityState.low && waterTimeAboveThreashold >= 1f) {
            waterState = UtilityState.ok;
            GetComponent<PubSubSender>().Publish("water.ok");
        } else if (waterState == UtilityState.ok && waterTimeAboveThreashold <= 0f) {
            waterState = UtilityState.low;
            GetComponent<PubSubSender>().Publish("water.low");
        }

        powerTMP.SetText("" + power);
        waterTMP.SetText("" + water);

        GetComponent<PubSubSender>().Publish("power.level", power);
        GetComponent<PubSubSender>().Publish("water.level", water);
    }
}

public enum UtilityState {
    ok,
    low
}