using System.Collections.Generic;
using UnityEngine;

public class TileBase : MonoBehaviour
{
    //타일의 속성(열거형)
    public TileType TileType;
    public CookType cookType;
    
    public ItemBase Item;
    private ITileLogicBase tileLogic;
    private ITileLogicBase TileLogic => tileLogic;
}
