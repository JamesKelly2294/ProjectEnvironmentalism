using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OilSlickType
{
    Land,
    Sea
}

public enum OilSlickLevel
{
    Land,
    Sea1,
    Sea2,
    Sea3,
    Sea4,
}

public class OilSlick : MonoBehaviour
{
    public OilSlickType type;
    public OilSlickLevel level;

    public SpriteRenderer OilSlickSprite;
    public Color NonPurchasableColor = new Color(1, 1, 1, 6.0f / 100.0f);
    public SelectableSprite Selectable;
    public bool Purchasable = true;

    public void SetPurchasable(bool purchasable)
    {
        if(Purchasable == purchasable) { return; }
        Purchasable = purchasable;
        UpdatePurchasableState();
    }

    void UpdatePurchasableState()
    {
        if (!Purchasable)
        {
            Selectable.BoxHighlightable = false;
            Selectable.BoxSelectable = false;
            Selectable.Selectable = false;
            Selectable.Highlightable = false;

            OilSlickSprite.color = NonPurchasableColor;
        }
        else
        {
            Selectable.BoxHighlightable = true;
            Selectable.BoxSelectable = true;
            Selectable.Selectable = true;
            Selectable.Highlightable = true;

            OilSlickSprite.color = Color.white;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdatePurchasableState();
    }
}
