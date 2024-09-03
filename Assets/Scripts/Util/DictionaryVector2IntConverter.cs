using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryVector2IntConverter : JsonConverter<Dictionary<Vector2Int, NodeData>>
{
    public override void WriteJson(JsonWriter writer, Dictionary<Vector2Int, NodeData> value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        foreach (var kvp in value)
        {
            writer.WritePropertyName($"({kvp.Key.x}, {kvp.Key.y})");
            serializer.Serialize(writer, kvp.Value);
        }
        writer.WriteEndObject();
    }

    public override Dictionary<Vector2Int, NodeData> ReadJson(JsonReader reader, Type objectType, Dictionary<Vector2Int, NodeData> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var dictionary = new Dictionary<Vector2Int, NodeData>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                var key = reader.Value.ToString().Trim('(', ')').Split(',');
                var vectorKey = new Vector2Int(int.Parse(key[0]), int.Parse(key[1]));
                reader.Read();
                var value = serializer.Deserialize<NodeData>(reader);
                dictionary[vectorKey] = value;
            }

            if (reader.TokenType == JsonToken.EndObject)
            {
                break;
            }
        }

        return dictionary;
    }
}