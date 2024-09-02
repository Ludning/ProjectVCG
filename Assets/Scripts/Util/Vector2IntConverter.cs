using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector2IntConverter : JsonConverter<Vector2Int>
{
    public override void WriteJson(JsonWriter writer, Vector2Int value, JsonSerializer serializer)
    {
        // (x, y) 형식으로 직렬화
        writer.WriteValue($"({value.x}, {value.y})");
    }

    public override Vector2Int ReadJson(JsonReader reader, System.Type objectType, Vector2Int existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        Debug.Log(reader);
        Debug.Log(reader.Value);
        // (x, y) 형식의 문자열을 Vector2Int로 변환
        var value = reader.Value.ToString().Trim('(', ')');
        var parts = value.Split(',');
        return new Vector2Int(int.Parse(parts[0]), int.Parse(parts[1]));
    }
}
