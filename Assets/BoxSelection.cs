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

    private Transform highlightedTransform;
    private Transform selectedTransform;

    private SelectableSprite selectedObject;

    public void ToggleSelect(PubSubListenerEvent e)
    {
        EvaluateSelectToggle(e);
    }

    public void Highlight(PubSubListenerEvent e)
    {
        isHighlighted = true;
        EvaluateHighlightToggle(e);
    }

    public void Unhighlight(PubSubListenerEvent e)
    {
        isHighlighted = false;
        EvaluateHighlightToggle(e);
    }

    private void EvaluateSelectToggle(PubSubListenerEvent e)
    {
        BoxSelectionSprite.color = SelectedColor;

        SelectableSprite incomingSprite = (SelectableSprite)e.value;
        Transform incomingTransform = incomingSprite.transform;

        if (incomingSprite == selectedObject)
        {
            isSelected = !isSelected;
        }
        else
        {
            isSelected = true;
        }

        selectedObject = incomingSprite;

        if (!isSelected)
        {
            BoxSelectionSprite.enabled = false;
            selectedTransform = null;
        }
        else
        {
            BoxSelectionSprite.transform.position = incomingTransform.position;
            selectedTransform = incomingTransform;
            BoxSelectionSprite.enabled = true;
        }
    }

    private void EvaluateHighlightToggle(PubSubListenerEvent e)
    {
        BoxHighlightSprite.color = HighlightedColor;

        SelectableSprite incomingSprite = (SelectableSprite)e.value;
        Transform incomingTransform = incomingSprite.transform;

        if (!isHighlighted)
        {
            BoxHighlightSprite.enabled = false;
            highlightedTransform = null;
        }
        else
        {
            BoxHighlightSprite.transform.position = incomingTransform.position;
            highlightedTransform = transform;
            BoxHighlightSprite.enabled = true;
        }
    }
}
