using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum PositionType
{
    Null,
    Player,
    Item,
    Npc
}

public class TableManager : MonoBehaviour
{
    [SerializeField] private Transform tableParent;
    [SerializeField] private Transform tileParent;
    [SerializeField] private Transform itemParent;
    [SerializeField] private Transform npcParent;
    
    [SerializeField] private PlayerController Controller;
    [SerializeField] private PlayerInventory Inventory;
    
    private Dictionary<Vector2Int, TileBase> map = new Dictionary<Vector2Int, TileBase>();
    public Dictionary<Vector2Int, TileBase> Map => map;

    public void InitTable(TableData tableData)
    {
        tableParent.position = new Vector3((-tableData.Size.x + 1) / 2f, 0, 2);
        
        foreach (var tileData in tableData.Table)
        {
            Vector2Int position = Vector2IntConverter.IntToVec2(tableData.Size, tileData.Key);
            GameObject tilePrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("Tile");
            GameObject tile = Instantiate(tilePrefab, tileParent);
            tile.transform.localPosition = new Vector3(position.x, 0, position.y);
            TileBase tileBase = tile.GetComponent<TileBase>();
            string levelKey = tableData.LevelDataDictionary.GetValueOrDefault(tileData.Key);
            tileBase.InitTile(tileData.Value, levelKey);
            map.Add(position, tileBase);
            
            if (tileData.Value.IsSpawnObject && tileData.Value.SpawnObjectType != ItemType.NULL)
            {
                string itemIndex = ((int)tileData.Value.SpawnObjectType).ToString();
                FoodData data = DataManager.Instance.GetGameData<FoodData>(itemIndex);
                GameObject foodPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(data.PrefabName);
                tileBase.IngredientItem = Instantiate(foodPrefab, itemParent, false);
                tileBase.IngredientItem.transform.position = GetTilePosition(position, PositionType.Item);
            }
            if (tileData.Value.IsSpawnNPC && tileData.Value.SpawnNpcType != NpcType.Null)
            {
                string npcIndex = ((int)tileData.Value.SpawnNpcType).ToString();
                NpcData data = DataManager.Instance.GetGameData<NpcData>(npcIndex);
                GameObject npcPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(data.PrefabName);
                tileBase.IngredientItem = Instantiate(npcPrefab, npcParent, false);
                tileBase.IngredientItem.transform.position = GetTilePosition(position, PositionType.Npc);
            }
        }
        Vector3 playerPosition = GetTilePosition(tableData.PlayerPosition, PositionType.Null);
        Controller.Init(tableData.PlayerPosition, playerPosition, tableData.PlayerDirection);
        Inventory.Init();
    }

    public void ResetTable(TableData tableData)
    {
        Debug.Log("스테이지 초기화");
        Clear();
        InitTable(tableData);
    }

    public TileBase PeekTile(Vector2Int position)
    {
        return TryGetTile(position, out TileBase tile) ? tile : null;
    }
    
    /// <summary>
    ///타일 타입을 가져오는 함수
    /// </summary>
    private TileType GetTileType(Vector2Int position)
    {
        if (Map.TryGetValue(position, out TileBase tileBase))
        {
            return tileBase.TileType;
        }
        return TileType.EMPTY;
    }
    
    /// <summary>
    /// 타일 위치를 반환받는 함수
    /// </summary>
    public Vector3 GetTilePosition(Vector2Int position, PositionType type)
    {
        if (Map.TryGetValue(position, out TileBase tileBase))
        {
            switch (type)
            {
                case PositionType.Player:
                    return tileBase.transform.position + tileBase.transform.up * GameManager.Instance.PlayerPositionAdditive;
                case PositionType.Item:
                    return tileBase.transform.position + tileBase.transform.up * GameManager.Instance.ItemPositionAdditive;
                case PositionType.Npc:
                    return tileBase.transform.position + tileBase.transform.up * GameManager.Instance.NpcPositionAdditive;
                default:
                    return tileBase.transform.position;
            }
        }
        return Vector3.zero;
    }

    /// <summary>
    /// TileBase를 반환받는 함수
    /// </summary>
    private bool TryGetTile(Vector2Int position, out TileBase tile)
    {
        if (Map.TryGetValue(position, out TileBase temp))
        {
            tile = temp;
            return true;
        }
        else
        {
            tile = null;
            return false;
        }
    }
    
    /// <summary>
    /// 타일 아이템을 꺼내오는 함수
    /// </summary>
    public ItemBase PopTileItem(Vector2Int position)
    {
        return TryGetTile(position, out TileBase tile) ? tile.TakeItem() : null;
    }

    /// <summary>
    /// 타일의 아이템 이름 리스트를 반환하는 함수
    /// </summary>
    public bool TryGetTileItemTypeList(Vector2Int position, out List<ItemType> itemTypeList)
    {
        if (TryGetTile(position, out TileBase tile))
        {
            itemTypeList = tile.GetItemTypeList();
            return true;
        }
        itemTypeList = null;
        return false;
    }
    
    /// <summary>
    /// 타일에 아이템을 넣는 함수
    /// </summary>
    public void PushTileItem(Vector2Int position, ItemBase item)
    {
        Map[position].SetItem(item);
        item.transform.position = GetTilePosition(position, PositionType.Item);
    }

    public void RotateTable(Quaternion quaternion)
    {
        //Vector3 angle = new Vector3(0, 90f, -38.445f);
        tableParent.rotation = quaternion;
    }
    
    public void Clear()
    {
        foreach (var tileBase in map.Values)
        {
            if (tileBase == null)
            {
                continue;
            }
            
            tileBase.ClearItem();
            Destroy(tileBase.gameObject);
        }
        map.Clear();
    }
}
