using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingUnit : LevelUnitBase
{
    private string _levelKey;
    private string _tileKey;
    public ServingUnit(string levelKey, string tileKey, Transform uiParent, NodeData nodeData)
    {
        _levelKey = levelKey;
        _tileKey = tileKey;
        InitUIElement(uiParent, nodeData);
    }
    private void InitUIElement(Transform uiParent, NodeData nodeData)
    {
        GameObject prefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("ServingUnitUIElement");
        GameObject uiObject = Object.Instantiate(prefab, uiParent);
        ServingUnitUIElement temp = uiObject.GetComponent<ServingUnitUIElement>();
        temp.Init(_levelKey, nodeData);
        LevelUnitUIElement = temp;
    }
    public override bool CheakLevel(TileBase tileBase, BlockLogicType type)
    {
        if (type != BlockLogicType.PopItem)
            return false;
        if(string.IsNullOrWhiteSpace(tileBase.LevelKey))
            return false;
        
        if(string.IsNullOrWhiteSpace(_tileKey))
            return true;
        return tileBase.LevelKey == _tileKey;
    }
}
