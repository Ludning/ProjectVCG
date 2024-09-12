using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArriveUnitUIElement : LevelUnitUIElement
{
    [SerializeField] private TextMeshProUGUI ArriveContext;
    public override void Init(string levelDataKey)
    {
        LevelData levelData = DataManager.Instance.GetGameData<LevelData>(levelDataKey);
        ArriveContext.text = levelData.DisplayContext;
    }
}
