using System.Collections.Generic;
using UnityEngine;

public class TileBase : MonoBehaviour
{
    //타일의 속성(열거형)
    public TileType TileType;
    public TileAttributeType TileAttributeType;
    
    //아이템
    public ItemBase Item;
    private ITileLogicBase tileLogic;
    private ITileLogicBase TileLogic => tileLogic;

    public void InitTile(string tileName)
    {
        TileData tileData = DataManager.Instance.GetGameData<TileData>(tileName);
        
        TileType = tileData.TileType;
        if (tileData.Moveable)
        {
            TileAttributeType |= TileAttributeType.Moveable;
        }
        if (tileData.Pushable)
        {
            TileAttributeType |= TileAttributeType.Pushable;
        }
        if (tileData.Popable)
        {
            TileAttributeType |= TileAttributeType.Popable;
        }
        if (tileData.Stackable)
        {
            TileAttributeType |= TileAttributeType.Stackable;
        }
    }
    public TileAttributeType GetTileAttributes(bool walkable, bool pushable, bool popable, bool stackable)
    {
        TileAttributeType attributes = TileAttributeType.None;

        if (walkable)
        {
            attributes |= TileAttributeType.Moveable;
        }
        if (pushable)
        {
            attributes |= TileAttributeType.Pushable;
        }
        if (popable)
        {
            attributes |= TileAttributeType.Popable;
        }
        if (stackable)
        {
            attributes |= TileAttributeType.Stackable;
        }

        return attributes;
    }
    public virtual void OnSetItemExcute()
    {

    }
    public virtual void OnItemSpwan()
    {

    }

    public void HighlightTile()
    {
        
    }
    public void UnhighlightTile()
    {
        
    }
}
