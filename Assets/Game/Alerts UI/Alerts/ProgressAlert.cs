using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressAlert: MonoBehaviour
{

    public ProgressBar progressBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProgressDidUpdate(PubSubListenerEvent e) {
        progressBar.percent = ((float) e.value) / 100f;
    }
}
