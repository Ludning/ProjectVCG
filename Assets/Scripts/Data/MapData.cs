using System;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    public Dictionary<string, TableData> TableData;
    
    [SerializeField] private Vector2Int position;
    public Vector2Int Position
    {
        get => position;
        set => position = value;
    }

    
}

public class TableData
{
    public Dictionary<Vector2Int, TileData> Table;
}

public class TileData
{
    public TileType tileType;
    public string itemName;
}
