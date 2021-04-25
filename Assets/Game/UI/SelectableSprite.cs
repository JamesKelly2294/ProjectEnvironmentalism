using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ISelectableSpriteDelegate
{
    void OnSelect();
    void OnDeelect();
    void OnHighlight();
    void OnUnhighlight();
}

[RequireComponent(typeof(PubSubSender))]
[RequireComponent(typeof(Collider2D))]
public class SelectableSprite : MonoBehaviour
{
    public bool Selectable = true;
    public bool Highlightable = true;
    public bool BoxHighlightable = false;
    public bool BoxSelectable = false;

    private bool isSelected = false;
    private bool isHighlighted = false;

    public string SelectionType = "selectablesprite";

    // Start is called before the first frame update
    void Start()
    {
        _sender = GetComponent<PubSubSender>();
    }

    private PubSubSender _sender;

    public void Selected()
    {
        if(isSelected)
        {
            return;
        }
        isSelected = true;
        _sender.Publish(SelectionType + ".gameobject.select", this.gameObject);
    }

    public void Highlighted()
    {
        if (isHighlighted)
        {
            return;
        }
        isHighlighted = true;
        _sender.Publish(SelectionType + ".gameobject.highlight", this.gameObject);
    }

    public void Deselected()
    {
        if (!isSelected)
        {
            return;
        }
        isSelected = false;
        _sender.Publish(SelectionType + ".gameobject.deselect", this.gameObject);
    }

    public void Unhighlighted()
    {
        if (!isHighlighted)
        {
            return;
        }
        isHighlighted = false;
        _sender.Publish(SelectionType + ".gameobject.unhighlight", this.gameObject);
    }

    public void OnMouseEnter()
    {
        if (Highlightable)
        {
            _sender.Publish("button.highlight.begin");
        }
        if(BoxHighlightable)
        {
            _sender.Publish("boxselectable.highlight.toggle", this);
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
            _sender.Publish("boxselectable.highlight.toggle", this);
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
