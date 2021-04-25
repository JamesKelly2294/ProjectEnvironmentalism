using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSlick : MonoBehaviour
{
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
