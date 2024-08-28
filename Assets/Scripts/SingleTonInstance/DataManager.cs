using System.Collections.Generic;
using Frameworks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DataManager : SingletonMonoBehaviour<DataManager>
{
    private GameData _gameData;
    private AssetAddressData _assetAddressData;
    private MapData _mapData;
    
    private const string GameDataJsonPath = "Data/GameData";
    private const string AddressDataJsonPath = "Data/AssetAddress";
    private const string MapDataJsonPath = "Data/MapData";
    
    public T GetGameData<T>(string key) where T : class, new()
    {
        Dictionary<string, T> dictionary = GetGameDataDictionary<T>();
        return dictionary?.GetValueOrDefault(key);
    }

    public Dictionary<string, T> GetGameDataDictionary<T>() where T : class
    {
        if (_gameData == null)
        {
            TextAsset jsonFile = Addressables.LoadAssetAsync<TextAsset>(GameDataJsonPath).WaitForCompletion();
            _gameData = JsonConvert.DeserializeObject<GameData>(jsonFile.text);
        }

        switch (typeof(T).Name)
        {
            case "PcData":
                return _gameData.Pc as Dictionary<string, T>;
            case "NpcData":
                return _gameData.Npc as Dictionary<string, T>;
            case "CodingBlockData":
                return _gameData.CodingBlock as Dictionary<string, T>;
            case "FeedbackData":
                return _gameData.Feedback as Dictionary<string, T>;
            case "StageData":
                return _gameData.Stage as Dictionary<string, T>;
            case "StageGoalData":
                return _gameData.StageGoal as Dictionary<string, T>;
            case "Tile_SpaceData":
                return _gameData.Tile_Space as Dictionary<string, T>;
            case "Tile_FoodData":
                return _gameData.Tile_Food as Dictionary<string, T>;
        }
        return null;
    }

    public string GetAssetAddress<T>(string key)
    {
        if (_assetAddressData == null)
        {
            TextAsset jsonFile = Addressables.LoadAssetAsync<TextAsset>(AddressDataJsonPath).WaitForCompletion();
            _assetAddressData = JsonConvert.DeserializeObject<AssetAddressData>(jsonFile.text);
        }

        if(typeof(T) == typeof(GameObject))
            return _assetAddressData.GameObject.GetValueOrDefault(key);
        if (typeof(T) == typeof(Material))
            return _assetAddressData.Material.GetValueOrDefault(key);

        return null;
    }
    
    public TableData GetTableData(string key)
    {
        if (_mapData == null)
        {
            TextAsset jsonFile = Addressables.LoadAssetAsync<TextAsset>(MapDataJsonPath).WaitForCompletion();
            _mapData = JsonConvert.DeserializeObject<MapData>(jsonFile.text);
        }

        return _mapData.TableData.GetValueOrDefault(key);
    }
}
