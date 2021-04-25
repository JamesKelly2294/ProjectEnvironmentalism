using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OilExtractor : MonoBehaviour
{
    public OilSlick ExtractedOilSlick { get; protected set; }

    public GameObject oilDerrickGraphic;
    public GameObject oilRigGraphic;

    public Slider OilReservesSlider;
    public Slider OilStorageSlider;

    public float MaxOilReserves = 1200;
    public float CurrentOilReserves;
    public float MaxOilStorage = 100;
    public float CurrentOilStorage = 0;
    [Range(0, 50)]
    public float OilExtractionRate = 10; // units per second

    private ResourceManager _resourceManager;

    // Start is called before the first frame update
    void Start()
    {
        CurrentOilReserves = MaxOilReserves;

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
        }
        else
        {
            oilRigGraphic.SetActive(true);
            oilDerrickGraphic.SetActive(false);
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
        float extractionRate = OilExtractionRate * Time.deltaTime;
        float extractableOil = CurrentOilReserves < extractionRate ? CurrentOilReserves : extractionRate;
        float storableOil = MaxOilStorage - CurrentOilStorage;
        float extractedOil = storableOil < extractableOil ? storableOil : extractableOil;
        CurrentOilReserves -= extractedOil;
        CurrentOilStorage += extractedOil;
    }

    private void OnDestroy()
    {
        _resourceManager.UnregisterOilExtractor(this);
    }
}
