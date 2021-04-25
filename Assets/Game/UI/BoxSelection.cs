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

    private SelectableSprite selectedObject;
    private SelectableSprite highlightedObject;

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
            incomingSprite = selectedObject;
        }

        if(!incomingSprite)
        {
            return;
        }

        if (incomingSprite == selectedObject)
        {
            isSelected = !isSelected;
        }
        else
        {
            isSelected = true;
            if (selectedObject)
            {
                selectedObject.Deselected();
            }
        }

        selectedObject = incomingSprite;
        if (!isSelected)
        {
            BoxSelectionSprite.enabled = false;
            selectedObject.Deselected();
            selectedObject = null;
        }
        else
        {
            BoxSelectionSprite.transform.position = incomingSprite.transform.position;
            selectedObject.Selected();
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
            incomingSprite = highlightedObject;
        }

        if (!incomingSprite)
        {
            return;
        }

        if (incomingSprite == highlightedObject)
        {
            isHighlighted = !isHighlighted;
        }
        else
        {
            isHighlighted = true;
            if (highlightedObject)
            {
                highlightedObject.Unhighlighted();
            }
        }

        highlightedObject = incomingSprite;
        if (!isHighlighted)
        {
            BoxHighlightSprite.enabled = false;
            highlightedObject.Unhighlighted();
            highlightedObject = null;
        }
        else
        {
            BoxHighlightSprite.transform.position = incomingSprite.transform.position;
            highlightedObject.Highlighted();
            BoxHighlightSprite.enabled = true;
        }
    }
}
