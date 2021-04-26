using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableInGameBuild : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        Destroy(gameObject);
#else
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            child.gameObject.SetActive(true);
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
