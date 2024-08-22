using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DataManager : SingleTonMono<DataManager>
{
    private GameData _gameData;
    private AssetAddressData _assetAddressData;
    
    const string _dataJsonPath = "Data/GameData";
    const string _addressJsonPath = "Data/AssetAddress";
    
    public T GetGameData<T>(string key) where T : class, new()
    {
        Dictionary<string, T> dictionary = GetGameDataDictionary<T>();
        return dictionary?.GetValueOrDefault(key);
    }

    public Dictionary<string, T> GetGameDataDictionary<T>() where T : class
    {
        if (_gameData == null)
        {
            TextAsset jsonFile = Addressables.LoadAssetAsync<TextAsset>(_dataJsonPath).WaitForCompletion();
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
            TextAsset jsonFile = Addressables.LoadAssetAsync<TextAsset>(_addressJsonPath).WaitForCompletion();
            _assetAddressData = JsonConvert.DeserializeObject<AssetAddressData>(jsonFile.text);
        }

        if(typeof(T) == typeof(GameObject))
            return _assetAddressData.GameObject.GetValueOrDefault(key);
        if (typeof(T) == typeof(Material))
            return _assetAddressData.Material.GetValueOrDefault(key);

        return null;
    }
}
