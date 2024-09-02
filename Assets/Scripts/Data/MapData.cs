using System;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    public Dictionary<string, TableData> TableData;
}

public class TableData
{
    public Dictionary<Vector2Int, string> Table;
}

public class NodeData
{
    public string nodeName;
    public bool IsPlayerPosition;
    public bool SpawnObject;
}