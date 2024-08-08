using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

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

    public TileType GetTileType(int x, int y)
    {
        return map[x + y].TileType;
    }
    public ItemBase GetTileItem(int x, int y)
    {
        return map[x + y].Item;
    }
}
