using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TableData
{
    public Vector2Int Size;
    
    public Vector2Int PlayerPosition;
    public Direction PlayerDirection;
    
    public Dictionary<int, NodeData> Table;
}
