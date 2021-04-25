using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    BoxSelection _boxSelection;
    // Start is called before the first frame update
    void Start()
    {
        _boxSelection = FindObjectOfType<BoxSelection>();
    }

    public void BuildOilExtractor()
    {
        var oilSlickSelectedSprite = _boxSelection.SelectedObject as SelectableSprite;
        if (oilSlickSelectedSprite == null)
        {
            Debug.Log("There is not currently an oil extactor selected.");
            return;
        }

        var oilSlick = oilSlickSelectedSprite.gameObject.GetComponent<OilSlick>();

        if(oilSlick == null)
        {
            Debug.Log("There is not currently an oil extactor selected.");
            return;
        }
        else
        {

        }
    }
}
