using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Country
{
    UnitedStates,
    Mexico,
    Cuba
}

public class Government : MonoBehaviour
{
    public string Name;
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
