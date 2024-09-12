using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingUnit : LevelUnitBase
{
    public ServingUnit(LevelData levelData) : base(levelData)
    {
        InitUIElement();
    }
    public override void InitUIElement()
    {
        GameObject prefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("ServingUnitUIElement");
        GameObject uiObject = Object.Instantiate(prefab);
        ServingUnitUIElement temp = uiObject.GetComponent<ServingUnitUIElement>();
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
