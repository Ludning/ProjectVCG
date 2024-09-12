using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServingUnitUIElement : LevelUnitUIElement
{
    [SerializeField] private Image NpcImage;
    [SerializeField] private Image ItemImage;
    public override void Init(string levelDataKey)
    {
        LevelData levelData = DataManager.Instance.GetGameData<LevelData>(levelDataKey);
        FoodData foodData = DataManager.Instance.GetGameData<FoodData>(levelData.Contents_Key);
        
        NpcImage.sprite = ResourceManager.Instance.LoadResourceWithCaching<Sprite>(levelData.Contents_Key);
        ItemImage.sprite = ResourceManager.Instance.LoadResourceWithCaching<Sprite>(foodData.PrefabName);
    }
}
