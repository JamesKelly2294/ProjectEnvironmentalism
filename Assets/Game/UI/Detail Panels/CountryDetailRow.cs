using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountryDetailRow : MonoBehaviour
{

    public Image image;
    public Sprite flagUS, flagCuba, flagMexico, flagHell;
    public TextMeshProUGUI title;
    public Country country;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void refresh() {
        if ( country == Country.UnitedStates ) {
            image.sprite = flagUS;
            title.SetText("United States");
        } else if ( country == Country.Cuba ) {
            image.sprite = flagCuba;
            title.SetText("Cuba");
        } else if ( country == Country.Mexico ) {
            image.sprite = flagMexico;
            title.SetText("Mexico");
        } else if (country == Country.Hell)
        {
            image.sprite = flagHell;
            title.SetText("Hell");
        }
    }

    public void Setup(Country country) {
        this.country = country;
        refresh();
    }
}
