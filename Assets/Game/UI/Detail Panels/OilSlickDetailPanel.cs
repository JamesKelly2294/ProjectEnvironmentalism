using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OilSlickDetailPanel : MonoBehaviour
{

    public OilSlick oilSlick;
    public Button buyButton;
    public TextMeshProUGUI buyButtonText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void refresh() {
        ResourceManager rm = GameObject.FindObjectOfType<ResourceManager>();

        float money = rm.CurrentMoney;
        bool allowedToBuy = true;
        float cost = 0;
        if (oilSlick.type == OilSlickType.Land) {
            allowedToBuy = rm.CurrentOilDerricks < rm.MaximumOilDerricks;
            // cost


        } else {
            allowedToBuy = rm.CurrentOilRigs < rm.MaximumOilRigs;



        }

    }

    public void SetOilSlick(OilSlick oilSlick) {
        this.oilSlick = oilSlick;
        refresh();
    }

    public void Buy() {
        PubSubSender sender = gameObject.GetComponent<PubSubSender>();
        sender.Publish("oilslick.purchase", oilSlick);
    }
}
