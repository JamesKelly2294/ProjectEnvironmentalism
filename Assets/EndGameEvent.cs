using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameEvent : MonoBehaviour
{
    public City HellCity;

    public SpriteRenderer SeaGraphic;
    public Color SeaGraphicDestroyedColor;

    public SpriteRenderer LandGraphic;
    public Color LandGraphicDestroyedColor;

    public SpriteRenderer BeachGraphic;
    public Color BeachGraphicDestroyedColor;
    
    public SpriteRenderer OutlineGraphic;
    public Color OutlineGraphicDestroyedColor;

    private ResourceManager _resourceManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _resourceManager = GameManager.FindObjectOfType<ResourceManager>();
    }

    public void TriggerEndGame()
    {
        HellCity.gameObject.SetActive(true);

        SeaGraphic.color = SeaGraphicDestroyedColor;
        LandGraphic.color = LandGraphicDestroyedColor;
        BeachGraphic.color = BeachGraphicDestroyedColor;
        OutlineGraphic.color = OutlineGraphicDestroyedColor;

        foreach(var city in _resourceManager.Cities)
        {
            if(city.Country != Country.Hell)
            {
                city.DestroyCity();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
