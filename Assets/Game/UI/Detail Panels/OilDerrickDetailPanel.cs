using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OilDerrickDetailPanel : MonoBehaviour
{

    public TextMeshProUGUI extraction;

    public ProgressSlider oilReserves, oilStorage;

    public OilExtractor oilExtractor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (oilExtractor == null) {
            return;
        }

        extraction.SetText("Extracts " + oilExtractor.OilExtractionRate.ToString() + "/s");
        oilReserves.progress = oilExtractor.CurrentOilReserves / oilExtractor.MaxOilReserves;
        oilStorage.progress = oilExtractor.CurrentOilStorage / oilExtractor.MaxOilStorage;
    }

    public void SetOilExtractor(OilExtractor extractor) {
        this.oilExtractor = extractor;
    }
}
