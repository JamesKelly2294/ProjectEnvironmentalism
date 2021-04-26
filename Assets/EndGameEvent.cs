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
        AudioManager.Instance.StopMusic();
        HellCity.gameObject.SetActive(true);
        
        foreach(var city in _resourceManager.Cities)
        {
            if(city.Country != Country.Hell)
            {
                city.DestroyCity(destructionAnimationDuration: 1.0f);
            }
        }

        StartCoroutine(AnimateWorldColorChange(5.0f));
    }

    IEnumerator AnimateWorldColorChange(float duration)
    {
        float elapsedTime = 0.0f;
        var seaGraphicStartColor = SeaGraphic.color;
        var landGraphicStartColor = LandGraphic.color;
        var beachGraphicStartColor = BeachGraphic.color;
        var outlineGraphicStartColor = OutlineGraphic.color;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            var pct = elapsedTime / duration;
            SeaGraphic.color = Color.Lerp(seaGraphicStartColor, SeaGraphicDestroyedColor, pct);
            LandGraphic.color = Color.Lerp(landGraphicStartColor, LandGraphicDestroyedColor, pct);
            BeachGraphic.color = Color.Lerp(beachGraphicStartColor, BeachGraphicDestroyedColor, pct);
            OutlineGraphic.color = Color.Lerp(outlineGraphicStartColor, OutlineGraphicDestroyedColor, pct);
            yield return new WaitForEndOfFrame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
