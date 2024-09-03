using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    [SerializeField] private Transform tableParent;
    
    private Dictionary<Vector2Int, TileBase> map = new Dictionary<Vector2Int, TileBase>();
    public Dictionary<Vector2Int, TileBase> Map => map;

    public Vector2Int startPosition;

    /*TableData tableData = new TableData();
    tableData.Table = new Dictionary<Vector2Int, NodeData>();
    for (int i = 0; i < 9; i++)
    {
        for (int k = 0; k < 9; k++)
        {
            tableData.Table.Add(new Vector2Int(i, k), new NodeData(){TileType = TileType.WALK});
        }
    }*/

    public void InitTable(string stageIndex)
    {
        //TODO
        //현재 스테이지 정보를 받아온 후 초기화
        TableData tableData = DataManager.Instance.GetTableData(stageIndex);
        
        foreach (var tileData in tableData.Table)
        {
            Vector2Int position = Vector2IntConverter.IntToVec2(tableData.Size, tileData.Key);
            
            GameObject tilePrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("Tile");
            GameObject tile = Instantiate(tilePrefab, tableParent);
            tile.transform.position = new Vector3(position.x, 0, position.y);
            TileBase tileBase = tile.GetComponent<TileBase>();
            tileBase.InitTile(tileData.Value.TileType);
            
            map.Add(position, tileBase);
        }
    }

    public void ClearTable()
    {
        
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
            return tileBase.transform.position + Vector3.up * 1.5f;
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
}
