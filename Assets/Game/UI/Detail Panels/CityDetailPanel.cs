using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CityDetailPanel : MonoBehaviour
{

    public TextMeshProUGUI title;
    public Image flag;

    public City city;

    public Sprite flagUS, flagCuba, flagMexico;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void refresh() {
        title.SetText(city.name);

        if (city.Country == Country.UnitedStates) {
            flag.sprite = flagUS;
        } else if (city.Country == Country.Cuba) {
            flag.sprite = flagCuba;
        } else if (city.Country == Country.Mexico) {
            flag.sprite = flagMexico;
        }

    }

    public void SetCity(City city) {
        this.city = city;
        refresh();
    }
}
