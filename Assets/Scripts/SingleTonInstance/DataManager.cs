using System.Collections.Generic;
using Frameworks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DataManager : Singleton<DataManager>
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
            case "ErrorMessageData":
                return _gameData.ErrorMessage as Dictionary<string, T>;
            case "StageData":
                return _gameData.Stage as Dictionary<string, T>;
            case "StageGoalData":
                return _gameData.StageGoal as Dictionary<string, T>;
            case "LevelData":
                return _gameData.Level as Dictionary<string, T>;
            case "TileData":
                return _gameData.Tile as Dictionary<string, T>;
            case "FoodData":
                return _gameData.Food as Dictionary<string, T>;
            case "CookingPropertyData":
                return _gameData.CookingProperty as Dictionary<string, T>;
            case "RecipeData":
                return _gameData.Recipe as Dictionary<string, T>;
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

        if (typeof(T) == typeof(GameObject))
            return _assetAddressData.GameObject.GetValueOrDefault(key);
        if (typeof(T) == typeof(Material))
            return _assetAddressData.Material.GetValueOrDefault(key);
        if (typeof(T) == typeof(Sprite))
            return _assetAddressData.Sprite.GetValueOrDefault(key);

        return null;
    }
    public MapData GetMapData()
    {
        if (_mapData == null)
        {
            TextAsset jsonFile = Addressables.LoadAssetAsync<TextAsset>(MapDataJsonPath).WaitForCompletion();
            if (jsonFile == null)
                return null;
            
            _mapData = JsonConvert.DeserializeObject<MapData>(jsonFile.text);//, settings);
        }

        return _mapData;
    }
    public TableData GetTableData(string key)
    {
        Debug.Log("GetTableData");
        if (_mapData == null)
        {
            TextAsset jsonFile = Addressables.LoadAssetAsync<TextAsset>(MapDataJsonPath).WaitForCompletion();
            //_mapData = JsonUtility.FromJson<MapData>(jsonFile.text);
            _mapData = JsonConvert.DeserializeObject<MapData>(jsonFile.text);
            
            //JsonSerializerSettings settings = new JsonSerializerSettings();
            //settings.Converters.Add(new Vector2IntConverter());
            //settings.Converters.Add(new DictionaryVector2IntConverter());

            //_mapData = JsonConvert.DeserializeObject<MapData>(jsonFile.text, settings);
        }
        
        return _mapData.TableData.GetValueOrDefault(key);
    }
}
