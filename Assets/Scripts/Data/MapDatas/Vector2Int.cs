/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Vector2Int
{
    public int x;
    public int y;

    public Vector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Vector2Int up => new Vector2Int(0, 1);
    public static Vector2Int down => new Vector2Int(0, -1);
    public static Vector2Int left => new Vector2Int(-1, 0);
    public static Vector2Int right => new Vector2Int(1, 0);
    public static Vector2Int zero => new Vector2Int(0, 0);
    
    public static Vector2Int operator +(Vector2Int v1, Vector2Int v2)
    {
        return new Vector2Int(v1.x + v2.x, v1.y + v2.y);
    }
    public static Vector2Int operator -(Vector2Int v1, Vector2Int v2)
    {
        return new Vector2Int(v1.x - v2.x, v1.y - v2.y);
    }

    public override string ToString()
    {
        return $"({x}, {y})";
    }
}
*/
