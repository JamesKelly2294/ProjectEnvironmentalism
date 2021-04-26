using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DebugMenu : MonoBehaviour
{
    BoxSelection _boxSelection;
    ResourceManager _resourceManager;
    // Start is called before the first frame update
    void Start()
    {
        _boxSelection = FindObjectOfType<BoxSelection>();
        _resourceManager = FindObjectOfType<ResourceManager>();
    }

    private OilSlick SelectedOilSlick()
    {
        var oilSlickSelectedSprite = _boxSelection.SelectedObject as SelectableSprite;
        if (oilSlickSelectedSprite == null)
        {
            return null;
        }

        return oilSlickSelectedSprite.gameObject.GetComponent<OilSlick>();
    }

    private OilExtractor SelectedOilExtractor()
    {
        var oilExtractorSelectedSprite = _boxSelection.SelectedObject as SelectableSprite;
        if (oilExtractorSelectedSprite == null)
        {
            return null;
        }

        return oilExtractorSelectedSprite.gameObject.GetComponent<OilExtractor>();
    }

    public void BuildOilExtractor()
    {
        var oilSlick = SelectedOilSlick();

        if (oilSlick == null)
        {
            Debug.Log("There is not currently an oil slick selected.");
            return;
        }
        else
        {
            _resourceManager.PurchaseOilSlick(oilSlick);
        }
    }
    
    public void SetupTradeRoute(string cityName)
    {
        var query = from c in _resourceManager.Cities
                    where c.Name == cityName
                    select c;
        var city = query.First();
        var oilExtractor = SelectedOilExtractor();
        if (city != null && oilExtractor != null)
        {
            oilExtractor.EstablishTradeRoute(city);
        }
    }

    public void UnlockOilSlickLevel(OilSlickLevel level)
    {
        var manager = FindObjectOfType<OilSlickManager>();
        manager.UnlockOilSlickLevel(level);
    }
}
