using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class TableManager : MonoBehaviour
{
    private TileBase[] map = new TileBase[100];
    public TileBase[] Map => map;

    public void Init()
    {
        map = new TileBase[100];
        //초기화 더 할꺼임
        //TODO
    }
    public Vector2 GetTilePosition(Vector2Int position)
    {
        //TODO
        return Vector2.zero;
    }

    public TileType GetTileType(Vector2Int position)
    {
        return map[position.x + position.y].TileType;
    }
    public ItemBase GetTileItem(Vector2Int position)
    {
        return map[position.x + position.y].Item;
    }
}
