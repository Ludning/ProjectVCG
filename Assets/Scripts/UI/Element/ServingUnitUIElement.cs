using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ServingUnitUIElement : LevelUnitUIElement
{
    [SerializeField] private Image NpcImage;
    [SerializeField] private Image ItemImage;
    public override void Init(string levelDataKey, NodeData nodeData = null)
    {
        LevelData levelData = DataManager.Instance.GetGameData<LevelData>(levelDataKey);
        
        ItemType itemType = StringEnumConverter.ParserStringToEnum<ItemType>(levelData.Contents_Key);
        FoodData foodData = DataManager.Instance.GetGameData<FoodData>(((int)itemType).ToString());

        if (nodeData == null)
        {
            Debug.Log($"ServingUnitUIElement의 NodeData가 Null입니다.");
            return;
        }
        NpcData npcData = DataManager.Instance.GetGameData<NpcData>(((int)nodeData.SpawnNpcType).ToString());
        
        NpcImage.sprite = ResourceManager.Instance.LoadResourceWithCaching<Sprite>(npcData.Icon);
        ItemImage.sprite = ResourceManager.Instance.LoadResourceWithCaching<Sprite>(foodData.Icon);
    }
}
