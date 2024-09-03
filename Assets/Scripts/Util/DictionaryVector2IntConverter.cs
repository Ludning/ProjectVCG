using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class DictionaryVector2IntConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(Dictionary<Vector2Int, NodeData>).IsAssignableFrom(objectType);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var dictionary = new Dictionary<Vector2Int, NodeData>();

        var jObject = JObject.Load(reader);
        foreach (var property in jObject.Properties())
        {
            // Key를 "(x, y)" 형식의 문자열에서 Vector2Int로 변환
            var keyString = property.Name.Trim('(', ')');
            var keyParts = keyString.Split(',');
            var x = int.Parse(keyParts[0].Trim());
            var y = int.Parse(keyParts[1].Trim());
            var key = new Vector2Int(x, y);

            // Value를 NodeData로 역직렬화
            var value = property.Value.ToObject<NodeData>(serializer);

            dictionary.Add(key, value);
        }

        return dictionary;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var dictionary = (Dictionary<Vector2Int, NodeData>)value;

        writer.WriteStartObject();

        foreach (var kvp in dictionary)
        {
            // Key를 "(x, y)" 형식의 문자열로 변환
            var key = $"({kvp.Key.x}, {kvp.Key.y})";
            writer.WritePropertyName(key);

            // Value를 직렬화
            serializer.Serialize(writer, kvp.Value);
        }

        writer.WriteEndObject();
    }
}