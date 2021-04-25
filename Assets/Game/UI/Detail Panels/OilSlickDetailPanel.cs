using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSlickDetailPanel : MonoBehaviour
{

    public OilSlick oilSlick;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void refresh() {

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
