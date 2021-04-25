using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Country
{
    UnitedStates,
    Mexico,
    Cuba
}

public class City : MonoBehaviour
{
    public string Name;
    [Range(0, 10_000_000)]
    public int Population;
    public Country Country;

    public TMPro.TextMeshPro Label;

    // Start is called before the first frame update
    void Start()
    {
        Label.text = Name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
