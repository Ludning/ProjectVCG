using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingUnit : LevelUnitBase
{
    private string _levelKey;
    private string _tileKey;
    public ServingUnit(string levelKey, string tileKey, Transform uiParent)
    {
        _levelKey = levelKey;
        _tileKey = tileKey;
        InitUIElement(uiParent);
    }
    private void InitUIElement(Transform uiParent)
    {
        GameObject prefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("ServingUnitUIElement");
        GameObject uiObject = Object.Instantiate(prefab, uiParent);
        ServingUnitUIElement temp = uiObject.GetComponent<ServingUnitUIElement>();
        temp.Init(_levelKey);
        LevelUnitUIElement = temp;
    }
    public override bool CheakLevel(TileBase tileBase)
    {
        if(string.IsNullOrWhiteSpace(tileBase.LevelKey))
            return false;
        return tileBase.LevelKey == _tileKey;
    }
}
