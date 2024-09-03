using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MapDataReader
{
    const string dataJsonPath = "Assets/Resources_Addressable/Data/MapData.json";
    public static MapData LoadMapData()
    {
        MapData mapData = DataManager.Instance.GetMapData();
        return mapData;
    }
    public static void SaveMapData(MapData mapData)
    {
        Debug.Log("SaveMapData");
        //var json = JsonUtility.ToJson(mapData);
        //var json = JsonConvert.SerializeObject(mapData);
        
        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.Converters.Add(new Vector2IntConverter());
        settings.Converters.Add(new DictionaryVector2IntConverter());

        string json = JsonConvert.SerializeObject(mapData, settings);
        
        //var json = JsonConvert.SerializeObject(mapData, Formatting.Indented);
        File.WriteAllText(dataJsonPath, json);
    }
    
    public static StageData LoadStageData(string key)
    {
        StageData stage = DataManager.Instance.GetGameData<StageData>(key);
        return stage;
    }

    public static NodeData LoadNodeData(TableData data, int x, int y)
    {
        return data.Table[new Vector2Int(x, y)];
    }
    
    public static Sprite LoadTileSprite(TileType type)
    {
        TileData data = DataManager.Instance.GetGameData<TileData>(((int)type).ToString());
        Sprite sprite = ResourceManager.Instance.LoadResource<Sprite>(data.ImageName);
        return sprite;
    }
}