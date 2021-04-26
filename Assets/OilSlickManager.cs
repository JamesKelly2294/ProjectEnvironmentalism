using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PubSubListener))]
public class OilSlickManager : MonoBehaviour
{
    public List<GameObject> oilSlickLevelGroups; // gameobject whose children are oil slicks of a certain tier
    public OilSlickLevel CurrentOilSlickLevel = OilSlickLevel.Land;

    public void UnlockOilSlickLevelEvent(PubSubListenerEvent e)
    {
        UnlockOilSlickLevel(CurrentOilSlickLevel + 1);
    }

    public void UnlockOilSlickLevel(OilSlickLevel type)
    {
        CurrentOilSlickLevel = type;
        for(int i = 0; i <= (int)type && i < oilSlickLevelGroups.Count; i++)
        {
            var parent = oilSlickLevelGroups[i];
            for (int j = 0; j < parent.transform.childCount; j++)
            {
                var oilSlick = parent.transform.GetChild(j).GetComponent<OilSlick>();
                if (oilSlick)
                {
                    oilSlick.SetPurchasable(true);
                }
            }
        }
    }

    public (bool, float) PriceToUnlockNextOilSlick() {
        ResourceManager rm = GameObject.FindObjectOfType<ResourceManager>();
        OilSlickManager osm = GameObject.FindObjectOfType<OilSlickManager>();
        int currentLevel = 1;
        if (osm.CurrentOilSlickLevel == OilSlickLevel.Sea2) {
            currentLevel = 2;
        } else if (osm.CurrentOilSlickLevel == OilSlickLevel.Sea3) {
            currentLevel = 3;
        } else if (osm.CurrentOilSlickLevel == OilSlickLevel.Sea4) {
            currentLevel = 4;
        }

        float levelCost = 100000f * Mathf.Pow(3, currentLevel); 
        return (rm.CurrentMoney >= levelCost, levelCost);
    }

    // Start is called before the first frame update
    void Start()
    {
        UnlockOilSlickLevel(CurrentOilSlickLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
