using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerTile : TileBase
{
    public ItemBase _targetItem;
    public override void OnSetItemExcute()
    {
        if(_targetItem.ItemName == Item.ItemName)
        {
            Debug.Log("");
        }
        else
        {
            Debug.Log("");
        }
    }
}