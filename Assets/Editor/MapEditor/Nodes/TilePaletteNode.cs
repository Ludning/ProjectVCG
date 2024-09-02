using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TilePaletteNode : MapNode
{
    public TileType TileType;
    public TilePaletteNode(TileType type)
    {
        SetStyle();
        SetTile(type);
        TileType = type;
    }
    private void SetStyle()
    {
        style.width = 50;
        style.height = 50;
        
        style.marginTop = 5;
        style.marginBottom = 5;
        style.marginLeft = 5;
        style.marginRight = 5;
        
        style.alignItems = Align.Center;         // 수평 중앙 정렬
        style.justifyContent = Justify.Center;   // 수직 중앙 정렬
        
        style.backgroundColor = new Color(0, 0.5f, 1, 0.2f);  // 연한 파란색
    }
}
