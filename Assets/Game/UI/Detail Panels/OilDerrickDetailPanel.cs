using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OilDerrickDetailPanel : MonoBehaviour
{

    public TextMeshProUGUI extraction;

    public ProgressSlider oilReserves, oilStorage;

    public OilExtractor oilExtractor;

    public GameObject productionTabButton, routesTabButton;
    public GameObject productionTab, routesTab;

    // Start is called before the first frame update
    void Start()
    {
        SwitchToProductionTab();
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

    public void SwitchToProductionTab() {
        productionTab.SetActive(true);
        routesTab.SetActive(false);
        productionTabButton.GetComponent<Image>().enabled = true;
        routesTabButton.GetComponent<Image>().enabled = false;
    }

    public void SwitchToRoutesTab() {
        productionTab.SetActive(false);
        routesTab.SetActive(true);
        productionTabButton.GetComponent<Image>().enabled = false;
        routesTabButton.GetComponent<Image>().enabled = true;
    }
}
