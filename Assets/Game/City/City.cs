using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class City : MonoBehaviour
{
    public string Name;
    [Range(0, 10_000_000)]
    public int Population;
    public Country Country;

    public float MaximumOilDemand = 1200; // units of oil
    public float CurrentOilDemand = 10; // units of oil

    [Range(0, 50)]
    public float OilDemandIncreaseRate = 10; // units per second

    public Slider OilDemandSlider;

    public TMPro.TextMeshPro Label;

    // Start is called before the first frame update
    void Start()
    {
        Label.text = Name;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateOilDemand();
        OilDemandSlider.normalizedValue = CurrentOilDemand / MaximumOilDemand;
    }

    void UpdateOilDemand()
    {
        CurrentOilDemand = Mathf.Min(MaximumOilDemand, CurrentOilDemand + OilDemandIncreaseRate * Time.deltaTime);
    }
}
