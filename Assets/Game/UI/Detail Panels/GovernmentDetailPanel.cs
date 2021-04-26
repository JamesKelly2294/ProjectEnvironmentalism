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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        environmentSlider.progress = government.environment;
        sentimentSlider.progress = government.sentiment;
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
}
