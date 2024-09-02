using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TableData
{
    public Vector2Int Size;
    public Dictionary<Vector2Int, NodeData> Table;
}
