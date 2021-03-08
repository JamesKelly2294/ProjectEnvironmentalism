using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    public GameObject bar;

    [Range(0f, 1f)]
    public float percent = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RectTransform progressRect = gameObject.transform.GetComponent<RectTransform>();
        float width = progressRect.sizeDelta.x * progressRect.localScale.x;
        bar.GetComponent<LayoutElement>().preferredWidth = percent * width;
    }
}
