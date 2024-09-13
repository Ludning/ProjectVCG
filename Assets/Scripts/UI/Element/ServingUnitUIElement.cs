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
        
        ItemType itemType = StringEnumConverter.ParserStringToEnum<ItemType>(levelData.Contents_Key);
        FoodData foodData = DataManager.Instance.GetGameData<FoodData>(((int)itemType).ToString());
        
        NpcData npcData = DataManager.Instance.GetGameData<NpcData>(levelData.NPC_Index);
        
        NpcImage.sprite = ResourceManager.Instance.LoadResourceWithCaching<Sprite>(npcData.Icon);
        ItemImage.sprite = ResourceManager.Instance.LoadResourceWithCaching<Sprite>(foodData.Icon);
    }
}
