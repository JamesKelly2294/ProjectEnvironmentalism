using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PubSubSender))]
public class InputManager : MonoBehaviour
{
    PubSubSender _sender;
    ResourceManager _rm;

    private void Awake()
    {
        _sender = GetComponent<PubSubSender>();
        _rm = FindObjectOfType<ResourceManager>();
    }
    // Update is called once per frame\\\\\\
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

        if(Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.M))
        {
            _rm.DepositFunds(5_000_000 * Time.deltaTime);
        }

    }
}
