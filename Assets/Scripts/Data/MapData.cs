using System;
using UnityEngine;

[Serializable]
public struct MapData
{
    [SerializeField] private Vector2Int position;
    public Vector2Int Position
    {
        get => position;
        set => position = value;
    }

    [SerializeField] private TileType tileType;
    public TileType TileType
    {
        get => tileType;
        set => tileType = value;
    }

    [SerializeField] private string itemName;
    public string ItemName
    {
        get => itemName;
        set => itemName = value;
    }
}
