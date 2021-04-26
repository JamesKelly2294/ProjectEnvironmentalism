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

    // Start is called before the first frame update
    void Start()
    {
        if (!Purchasable)
        {
            Selectable.BoxHighlightable = false;
            Selectable.BoxSelectable = false;
            Selectable.Selectable = false;
            Selectable.Highlightable = false;

            OilSlickSprite.color = NonPurchasableColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
