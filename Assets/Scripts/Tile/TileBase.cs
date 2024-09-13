using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TileBase : MonoBehaviour
{
    //타일의 속성(열거형)
    public TileType TileType;
    public TileAttributeType TileAttributeType;

    public string LevelKey;
    private int InventoryCount = 0;

    public ItemBase NoRimitItemMesh;
    
    public Stack<ItemBase> InventoryStack = new Stack<ItemBase>();

    public bool IsWalkAble => (TileAttributeType & TileAttributeType.Moveable) != 0;
    public bool IsPushAble => (TileAttributeType & TileAttributeType.Pushable) != 0;
    public bool IsPopAble  => (TileAttributeType & TileAttributeType.Popable)  != 0;
    public bool IsStackAble => (TileAttributeType & TileAttributeType.Stackable) != 0;
    public bool IsCookAble => (TileAttributeType & TileAttributeType.Cookable) != 0;
    public bool IsInventoryEmpty => (InventoryCount == 0) ? true : false;
    public bool IsInventoryEmptyOrFull => (InventoryCount == 0 || InventoryStack.Count == InventoryCount) ? true : false;


    public void InitTile(NodeData nodeData, string levelIndex)
    {
        LevelKey = levelIndex;
        string tileKey = ((int)nodeData.TileType).ToString();
        TileData tileData = DataManager.Instance.GetGameData<TileData>(tileKey);

        TileType = tileData.TileType;
        TileAttributeType =
            (tileData.Moveable ? TileAttributeType.Moveable : 0) |
            (tileData.Pushable ? TileAttributeType.Pushable : 0) |
            (tileData.Popable ? TileAttributeType.Popable : 0) |
            (tileData.Stackable ? TileAttributeType.Stackable : 0);

        InventoryCount = tileData.InventoryCount;

        /*if (nodeData.SpawnObjectType != ItemType.Null)
        {
            string itemIndex = ((int)nodeData.SpawnObjectType).ToString();
            FoodData data = DataManager.Instance.GetGameData<FoodData>(itemIndex);
            GameObject foodPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(data.PrefabName);
            NoRimitItemMesh = Instantiate(foodPrefab).GetComponent<ItemBase>();
            NoRimitItemMesh.transform.SetParent(transform, false);
            NoRimitItemMesh.transform.localPosition = transform.position + Vector3.up * 1.5f;
        }*/

        //TileLogic 설치
        InitTileMesh(tileData.PrefabName);
    }

    public void InitTileMesh(string tileName)
    {
        if (TileType == TileType.EMPTY)
            return;

        GameObject tilePrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(tileName);
        GameObject go = Instantiate(tilePrefab, transform);
        go.transform.localPosition = Vector3.zero;
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
    //아이템을 가져가는 함수
    public ItemBase TakeItem()
    {
        if (InventoryCount == -1)
            return Instantiate(NoRimitItemMesh.gameObject).GetComponent<ItemBase>();
        return InventoryStack.Count > 0 ? InventoryStack.Pop() : null;
    }
    //아이템을 놓는 함수
    public void SetItem(ItemBase item)
    {
        InventoryStack.Push(item);
        item.transform.SetParent(transform, false);
        item.transform.localPosition = transform.position + Vector3.up * 1.5f;
    }
    public void ClearItem()
    {
        foreach (var itemBase in InventoryStack)
            Destroy(itemBase);
        InventoryStack.Clear();
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
