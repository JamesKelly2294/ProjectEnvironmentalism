using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressSlider : MonoBehaviour
{

    public GameObject needle;

    [Range(0f, 1f)]
    public float progress;
    public float padding = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RectTransform needleRect = needle.GetComponent<RectTransform>();
        RectTransform selfRect = gameObject.GetComponent<RectTransform>();
        float offset = padding + (progress * (selfRect.rect.width - (2 * padding))) - (needleRect.rect.width / 2);
        needleRect.localPosition = new Vector3(offset, needleRect.localPosition.y, needleRect.localPosition.z);

    }
}
