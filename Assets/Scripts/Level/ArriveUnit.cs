using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//도달체크
public class ArriveUnit : LevelUnitBase
{
    public ArriveUnit(LevelData levelData) : base(levelData)
    {
        InitUIElement();
    }
    public override void InitUIElement()
    {
        GameObject prefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("ArriveUnitUIElement");
        GameObject uiObject = Object.Instantiate(prefab);
        ArriveUnitUIElement temp = uiObject.GetComponent<ArriveUnitUIElement>();
        temp.Init();
        _levelUnitUIElement = temp;
    }
    public override bool CheakLevel(TileBase tileBase)
    {
        if(string.IsNullOrWhiteSpace(tileBase.LevelKey))
            return false;
        return tileBase.LevelKey == _levelData.Tile_Key;
    }
}
