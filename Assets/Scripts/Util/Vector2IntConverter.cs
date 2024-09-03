using System;
using Newtonsoft.Json;
using UnityEngine;

public class Vector2IntConverter
{
    public static Vector2Int IntToVec2(Vector2Int size, int num)
    {
        int x = num % size.x; // num을 size.x로 나눈 나머지가 x 좌표가 됩니다.
        int y = num / size.x; // num을 size.x로 나눈 몫이 y 좌표가 됩니다.
        return new Vector2Int(x, y);
    }
    public static int Vec2ToInt(Vector2Int size, Vector2Int vec)
    {
        return vec.y * size.x + vec.x; // y 좌표에 size.x를 곱한 후, x 좌표를 더하면 num이 됩니다.
    }
}