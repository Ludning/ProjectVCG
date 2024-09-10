using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    [SerializeField] private Transform tableParent;
    [SerializeField] private Transform itemParent;
    
    private Dictionary<Vector2Int, TileBase> map = new Dictionary<Vector2Int, TileBase>();
    public Dictionary<Vector2Int, TileBase> Map => map;

    public void InitTable(TableData tableData)
    {
        foreach (var tileData in tableData.Table)
        {
            Vector2Int position = Vector2IntConverter.IntToVec2(tableData.Size, tileData.Key);
            GameObject tilePrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("Tile");
            GameObject tile = Instantiate(tilePrefab, tableParent);
            tile.transform.localPosition = new Vector3(position.x, 0, position.y);
            TileBase tileBase = tile.GetComponent<TileBase>();
            tileBase.InitTile(tileData.Value);
            map.Add(position, tileBase);
            
            if (tileData.Value.SpawnObjectType != ItemType.Null)
            {
                string itemIndex = ((int)tileData.Value.SpawnObjectType).ToString();
                FoodData data = DataManager.Instance.GetGameData<FoodData>(itemIndex);
                Debug.Log($"{tileData.Value.SpawnObjectType}");
                Debug.Log($"{itemIndex}");
                Debug.Log($"{data.PrefabName}");
                GameObject foodPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(data.PrefabName);
                tileBase.NoRimitItemMesh = Instantiate(foodPrefab).GetComponent<ItemBase>();
                tileBase.NoRimitItemMesh.transform.SetParent(itemParent, false);
                tileBase.NoRimitItemMesh.transform.position = GetTilePosition(position);
            }
        }
        
    }

    public void ClearTable()
    {
        //TODO
        Debug.Log("스테이지 초기화");
    }
    /// <summary>
    ///아이템이 없는지 체크하는 함수
    /// </summary>
    /*public bool CheakTileInventoryEmpty(Vector2Int position)
    {
        if (TryGetTile(position, out TileBase tile))
        {
            return tile.CheakTileAttribute(AttributeCheakType.InventoryEmpty);
        }
        return true;
    }*/

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
    public Vector3 GetTilePosition(Vector2Int position)
    {
        if (Map.TryGetValue(position, out TileBase tileBase))
        {
            return tileBase.transform.position + Vector3.up * 0.8f;
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
    public bool TryGetTileItemNameList(Vector2Int position, out List<string> itemNameList)
    {
        if (TryGetTile(position, out TileBase tile))
        {
            itemNameList = tile.GetItemNameList();
            return true;
        }
        itemNameList = null;
        return false;
    }
    /// <summary>
    /// 타일에 아이템을 넣는 함수
    /// </summary>
    public void PushTileItem(Vector2Int position, ItemBase item)
    {
        Map[position].SetItem(item);
    }
    public void Clear()
    {
        foreach (var tileBase in map.Values)
        {
            Destroy(tileBase.gameObject);
        }
        map.Clear();
    }
}
