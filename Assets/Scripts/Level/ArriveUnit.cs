using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//도달체크
public class ArriveUnit : LevelUnitBase
{
    private string _tileKey;
    public ArriveUnit(string tileKey, Transform uiParent)
    {
        _tileKey = tileKey;
        InitUIElement(uiParent);
    }
    private void InitUIElement(Transform uiParent)
    {
        GameObject prefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("ArriveUnitUIElement");
        GameObject uiObject = Object.Instantiate(prefab, uiParent);
        ArriveUnitUIElement temp = uiObject.GetComponent<ArriveUnitUIElement>();
        temp.Init();
        LevelUnitUIElement = temp;
    }
    public override bool CheakLevel(TileBase tileBase)
    {
        if(string.IsNullOrWhiteSpace(tileBase.LevelKey))
            return false;
        return tileBase.LevelKey == _tileKey;
    }
}
