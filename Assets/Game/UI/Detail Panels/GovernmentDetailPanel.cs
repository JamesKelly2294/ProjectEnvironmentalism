using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GovernmentDetailPanel : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI subtitle;

    public Image flag;

    public Government government;

    public Sprite flagUS, flagCuba, flagMexico;

    public ProgressSlider environmentSlider, sentimentSlider;

    public Button expandButton;
    public TextMeshProUGUI expandButtonText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        environmentSlider.progress = government.environment;
        sentimentSlider.progress = government.sentiment;

        
        if (government.hasGrantedExpandedOilRights) {
            expandButton.interactable = false;
            expandButtonText.SetText("Expand Oil Rights (Already Purchased)");
        } else {
            OilSlickManager osm = GameObject.FindObjectOfType<OilSlickManager>();
            (bool, float) affordability = osm.PriceToUnlockNextOilSlick();

            expandButton.interactable = affordability.Item1 && (government.sentiment >= 0.7);
            string buttonText = "Expand Oil Rights ($" + affordability.Item2.ToString("N2") + ")";
            if (government.sentiment <= 0.7) {
                buttonText += " (Requires sentiment >= 70%)";
            }
            expandButtonText.SetText(buttonText);
        }

    }

    void refresh() {
        subtitle.SetText(government.name);

        if (government.Country == Country.UnitedStates) {
            title.SetText("The U.S. Government");
            flag.sprite = flagUS;
        } else if (government.Country == Country.Cuba) {
            title.SetText("The Cuban Government");
            flag.sprite = flagCuba;
        } else if (government.Country == Country.Mexico) {
            title.SetText("The Mexican Government");
            flag.sprite = flagMexico;
        }

    }

    public void SetGovernment(Government government) {
        this.government = government;
        refresh();
    }

    public void ExpandOilRights () {
        government.ExpandOilRights();
    }
}
