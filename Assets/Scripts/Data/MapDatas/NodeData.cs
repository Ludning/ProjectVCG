using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeData
{
    public TileType TileType;
    public TileDetailType TileDetailType;
    public bool IsSpawnNPC;
    public NpcType SpawnNpcType;
    public bool IsSpawnObject;
    public ItemType SpawnObjectType;
}
