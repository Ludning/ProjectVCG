using System.Collections.Generic;
using UnityEngine;

public class TileBase : MonoBehaviour
{
    //타일의 속성(열거형)
    public TileType TileType;
    public CookType cookType;
    Stack<ItemBase> items = new Stack<ItemBase>();
    public ItemBase Item;

    public virtual void OnSetItemExcute()
    {

    }
    public virtual void OnItemSpwan()
    {

    }
}
