using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeterministicRandomStartRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Random.InitState(transform.position.GetHashCode());
        transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        Destroy(this);
    }
}
