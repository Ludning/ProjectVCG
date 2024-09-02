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
        var json = JsonConvert.SerializeObject(mapData, Formatting.Indented);
        File.WriteAllText(dataJsonPath, json);
    }
    
    public static StageData LoadStageData(string key)
    {
        StageData stage = DataManager.Instance.GetGameData<StageData>(key);
        return stage;
    }
    
    public static Sprite LoadTileSprite(TileType type)
    {
        TileData data = DataManager.Instance.GetGameData<TileData>(((int)type).ToString());
        Sprite sprite = ResourceManager.Instance.LoadResource<Sprite>(data.ImageName);
        return sprite;
    }
    public static TileType LoadTileType(int x, int y)
    {
        return TileType.WALK;
    }
}