using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MapDataReader
{
    const string dataJsonPath = "Assets/Resources_Addressable/Data/MapData.json";
    private static MapData LoadMapData()
    {
        //Tiles.Add(Resources.Load<Sprite>("Clear Illustration"));
        //Tiles.Add(Resources.Load<Sprite>("Drop Illustration"));
        //Tiles.Add(Resources.Load<Sprite>("Lift Illustration"));
        //Tiles.Add(Resources.Load<Sprite>("Start Illustration"));
        //Tiles.Add(Resources.Load<Sprite>("TurnLeft Illustration"));
        //Tiles.Add(Resources.Load<Sprite>("TurnRight Illustration"));
        //Tiles.Add(Resources.Load<Sprite>("WalkForward Illustration"));
        MapData mapData = new MapData();
        TableData tableData = new TableData();
        tableData.Table = new Dictionary<Vector2Int, string>();
        tableData.Table.Add(new Vector2Int(-1, -1), ((int)TileType.WALK).ToString());
        tableData.Table.Add(new Vector2Int(-1, 0), ((int)TileType.BLOCKING).ToString());
        tableData.Table.Add(new Vector2Int(-1, 1), ((int)TileType.WALK).ToString());
        tableData.Table.Add(new Vector2Int(0, -1), ((int)TileType.WALK).ToString());
        tableData.Table.Add(new Vector2Int(0, 0),  ((int)TileType.WALK).ToString());
        tableData.Table.Add(new Vector2Int(0, 1),  ((int)TileType.WALK).ToString());
        tableData.Table.Add(new Vector2Int(1, -1), ((int)TileType.WALK).ToString());
        tableData.Table.Add(new Vector2Int(1, 0),  ((int)TileType.WALK).ToString());
        tableData.Table.Add(new Vector2Int(1, 1), ((int)TileType.WALK).ToString());
        
        mapData.TableData.Add("3000", tableData);
        
        return mapData;
    }
    private static void SaveMapData(MapData mapData)
    {
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
        Debug.Log($"LoadTileSprite : {type}");
        TileData data = DataManager.Instance.GetGameData<TileData>(((int)type).ToString());
        Sprite sprite = ResourceManager.Instance.LoadResource<Sprite>(data.ImageName);
        return sprite;
    }
    public static TileType LoadTileType(int x, int y)
    {
        return TileType.WALK;
    }
}