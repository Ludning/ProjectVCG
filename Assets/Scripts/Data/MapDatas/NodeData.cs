using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeData
{
    public TileType TileType;
    public bool IsPlayerPosition;
    public Direction PlayerDirection;
    public bool IsSpawnObject;
    public ItemType SpawnObjectType;
}
