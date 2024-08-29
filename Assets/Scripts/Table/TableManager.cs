using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    [SerializeField] private Transform tableParent;
    
    private Dictionary<Vector2Int, TileBase> map = new Dictionary<Vector2Int, TileBase>();
    public Dictionary<Vector2Int, TileBase> Map => map;

    public Vector2Int startPosition;

    public void InitTable(string stageInfo)
    {
        //TODO
        //현재 스테이지 정보를 받아온 후 초기화
        //TableData tableData = DataManager.Instance.GetTableData(stageInfo);
        TableData tableData = new TableData();
        tableData.Table = new Dictionary<Vector2Int, string>();
        tableData.Table.Add(new Vector2Int(-1, -1), "10000");
        tableData.Table.Add(new Vector2Int(-1, 0), "10000");
        tableData.Table.Add(new Vector2Int(-1, 1), "10000");
        tableData.Table.Add(new Vector2Int(0, -1), "10000");
        tableData.Table.Add(new Vector2Int(0, 0), "10000");
        tableData.Table.Add(new Vector2Int(0, 1), "10000");
        tableData.Table.Add(new Vector2Int(1, -1), "10000");
        tableData.Table.Add(new Vector2Int(1, 0), "10000");
        tableData.Table.Add(new Vector2Int(1, 1), "10000");

        foreach (var tileData in tableData.Table)
        {
            GameObject tilePrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("Tile");
            GameObject tile = Instantiate(tilePrefab, tableParent);
            tile.transform.position = new Vector3(tileData.Key.x, 0, tileData.Key.y);
            TileBase tileBase = tile.GetComponent<TileBase>();
            tileBase.InitTile(tileData.Value);
            map.Add(tileData.Key, tileBase);
        }
    }

    public void ClearTable()
    {
        
    }
    /*public void Init(Dictionary<Vector2Int, TileBase> mapDictionary)
    {
        map = mapDictionary;
    }*/
    
    public Vector3 GetTilePosition(Vector2Int position)
    {
        if (Map.TryGetValue(position, out TileBase tileBase))
        {
            return tileBase.transform.position + Vector3.up * 1.5f;
        }
        return Vector3.zero;
    }

    public TileType GetTileType(Vector2Int position)
    {
        if (Map.TryGetValue(position, out TileBase tileBase))
        {
            return tileBase.TileType;
        }
        return TileType.Empty;
    }
    public ItemBase GetTileItem(Vector2Int position)
    {
        if (Map.TryGetValue(position, out TileBase tileBase))
        {
            return tileBase.Item;
        }
        return null;
    }
    public bool SetTileItem(Vector2Int position, ItemBase item)
    {
        if (Map.TryGetValue(position, out TileBase tileBase))
        {
            if (tileBase.Item)
            {
                Map[position].Item = item;
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
}
