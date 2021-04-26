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
