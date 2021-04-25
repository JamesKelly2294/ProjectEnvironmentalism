using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PubSubSender))]
public class InputManager : MonoBehaviour
{
    PubSubSender _sender;
    private void Awake()
    {
        _sender = GetComponent<PubSubSender>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            _sender.Publish("button.press.begin");
            _sender.Publish("boxselectable.select.toggle");
        }
        else if (Input.GetMouseButtonUp(1))
        {
            _sender.Publish("button.press.end");
        }
    }
}
