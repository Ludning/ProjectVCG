using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    [SerializeField]
    private Dictionary<Vector2Int, TileBase> map;
    public Dictionary<Vector2Int, TileBase> Map => map;

    public Vector2Int startPosition;

    public void Init()
    {
        //map = new TileBase[100];
        //초기화 더 할꺼임
        //TODO
    }
    public void Init(Dictionary<Vector2Int, TileBase> mapDictionary)
    {
        map = mapDictionary;
    }
    
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
    public CookType GetCookType(Vector2Int position)
    {
        if (Map.TryGetValue(position, out TileBase tileBase))
        {
            return tileBase.cookType;
        }
        return CookType.Empty;
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
