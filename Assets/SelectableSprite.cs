using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PubSubSender))]
[RequireComponent(typeof(Collider2D))]
public class SelectableSprite : MonoBehaviour
{
    public bool Selectable = true;
    public bool Highlightable = true;
    public bool BoxHighlightable = false;
    public bool BoxSelectable = false;

    public bool isSelected = false;
    public bool isHighlighted = false;

    // Start is called before the first frame update
    void Start()
    {
        _sender = GetComponent<PubSubSender>();
    }

    private PubSubSender _sender;
    
    public void OnMouseEnter()
    {
        if (Highlightable)
        {
            _sender.Publish("button.highlight.begin");
        }
        if(BoxHighlightable)
        {
            _sender.Publish("boxselectable.highlight.begin", this);
        }
    }

    public void OnMouseExit()
    {
        if (Highlightable)
        {
            _sender.Publish("button.highlight.end");
        }
        if (BoxHighlightable)
        {
            _sender.Publish("boxselectable.highlight.end", this);
        }
    }

    public void OnMouseDown()
    {
        if (Selectable)
        {
            _sender.Publish("button.press.begin");
        }
        if (BoxSelectable)
        {
            _sender.Publish("boxselectable.select.toggle", this);
        }
    }

    public void OnMouseUp()
    {
        if (Selectable)
        {
            _sender.Publish("button.press.end");
            _sender.Publish("button.select.toggle");
        }
    }
}
