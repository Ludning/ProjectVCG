using System;
using System.Collections.Generic;
using UnityEngine;

public class TileBase : MonoBehaviour
{
    //타일의 속성(열거형)
    public TileType TileType;
    public TileAttributeType TileAttributeType;

    private int InventoryCount = 0;

    public Stack<ItemBase> InventoryStack = new Stack<ItemBase>();

    private bool IsWalkAble => (TileAttributeType & TileAttributeType.Moveable) != 0;
    private bool IsPushAble => (TileAttributeType & TileAttributeType.Pushable) != 0;
    private bool IsPopAble  => (TileAttributeType & TileAttributeType.Popable)  != 0;
    private bool IsStackAble => (TileAttributeType & TileAttributeType.Stackable) != 0;
    private bool IsCookAble => (TileAttributeType & TileAttributeType.Cookable) != 0;
    private bool IsInventoryEmpty => (InventoryCount == 0) ? true : false;
    private bool IsInventoryEmptyOrFull => (InventoryCount == 0 || InventoryStack.Count == InventoryCount) ? true : false;

    public bool CheakTileAttribute(TileAttributeCheckType type)
    {
        switch (type)
        {
            case TileAttributeCheckType.WalkAble:
                return IsWalkAble;
            case TileAttributeCheckType.PushAble:
                return IsPushAble;
            case TileAttributeCheckType.PopAble:
                return IsPopAble;
            case TileAttributeCheckType.StackAble:
                return IsStackAble;
            case TileAttributeCheckType.CookAble:
                return IsCookAble;
            case TileAttributeCheckType.InventoryEmpty:
                return IsInventoryEmpty;
            case TileAttributeCheckType.InventoryEmptyOrFull:
                return IsInventoryEmptyOrFull;
            default:
                return false;
        }
    }
    
    public void InitTile(string tileName)
    {
        TileData tileData = DataManager.Instance.GetGameData<TileData>(tileName);
        
        TileType = tileData.TileType;
        TileAttributeType = 
            (tileData.Moveable ? TileAttributeType.Moveable : 0) |
            (tileData.Pushable ? TileAttributeType.Pushable : 0) |
            (tileData.Popable ? TileAttributeType.Popable : 0) |
            (tileData.Stackable ? TileAttributeType.Stackable : 0);
        
        InventoryCount = tileData.InventoryCount;
        
        //TileLogic 설치
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

    public List<string> GetItemNameList()
    {
        List<string> itemNames = new List<string>();

        foreach (ItemBase item in InventoryStack)
        {
            itemNames.Add(item.ItemName);
        }

        return itemNames;
    }
    public ItemBase TakeItem()
    {
        return InventoryStack.Count > 0 ? InventoryStack.Pop() : null;
    }
    public void SetItem(ItemBase item)
    {
        InventoryStack.Push(item);
        item.transform.SetParent(transform, false);
        item.transform.position = transform.position + Vector3.up * 1.5f;
    }
    public void ClearItem()
    {
        //TODO
    }
    public void OnItemSpawn()
    {

    }
    public void HighlightTile()
    {
        
    }
    public void UnhighlightTile()
    {
        
    }
}
