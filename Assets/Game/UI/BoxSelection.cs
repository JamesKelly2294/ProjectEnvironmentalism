using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PubSubListener))]
public class BoxSelection : MonoBehaviour
{
    public SpriteRenderer BoxSelectionSprite;
    public SpriteRenderer BoxHighlightSprite;

    public Color SelectedColor = Color.white;
    public Color HighlightedColor = new Color(1, 1, 1, 0.5f);

    private bool isHighlighted = false;
    private bool isSelected = false;

    public SelectableSprite SelectedObject { get; private set; }
    public SelectableSprite HighlightedObject { get; private set; }

    public void ToggleSelect(PubSubListenerEvent e)
    {
        EvaluateSelectToggle(e);
    }

    public void ToggleHighlight(PubSubListenerEvent e)
    {
        EvaluateHighlightToggle(e);
    }

    private void EvaluateSelectToggle(PubSubListenerEvent e)
    {
        BoxSelectionSprite.color = SelectedColor;

        SelectableSprite incomingSprite;
        if (e.value != null)
        {
            incomingSprite = (SelectableSprite)e.value;
        }
        else
        {
            incomingSprite = SelectedObject;
        }

        if(!incomingSprite)
        {
            return;
        }

        if (incomingSprite == SelectedObject)
        {
            isSelected = !isSelected;
        }
        else
        {
            isSelected = true;
            if (SelectedObject)
            {
                SelectedObject.Deselected();
            }
        }

        SelectedObject = incomingSprite;
        if (!isSelected)
        {
            Debug.Log("Deselect " + SelectedObject);
            BoxSelectionSprite.enabled = false;
            SelectedObject.Deselected();
            SelectedObject = null;
        }
        else
        {
            Debug.Log("Select " + SelectedObject);
            BoxSelectionSprite.transform.position = incomingSprite.transform.position;
            SelectedObject.Selected();
            BoxSelectionSprite.enabled = true;
        }
    }

    private void EvaluateHighlightToggle(PubSubListenerEvent e)
    {
        BoxHighlightSprite.color = HighlightedColor;

        SelectableSprite incomingSprite;
        if (e.value != null)
        {
            incomingSprite = (SelectableSprite)e.value;
        }
        else
        {
            incomingSprite = HighlightedObject;
        }

        if (!incomingSprite)
        {
            return;
        }

        if (incomingSprite == HighlightedObject)
        {
            isHighlighted = !isHighlighted;
        }
        else
        {
            isHighlighted = true;
            if (HighlightedObject)
            {
                HighlightedObject.Unhighlighted();
            }
        }

        HighlightedObject = incomingSprite;
        if (!isHighlighted)
        {
            BoxHighlightSprite.enabled = false;
            HighlightedObject.Unhighlighted();
            HighlightedObject = null;
        }
        else
        {
            BoxHighlightSprite.transform.position = incomingSprite.transform.position;
            HighlightedObject.Highlighted();
            BoxHighlightSprite.enabled = true;
        }
    }
}
