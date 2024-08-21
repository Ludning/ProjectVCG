using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MapData
{
    public Vector2Int Position;
    public TileType TileType;
    public string ItemName;
}
