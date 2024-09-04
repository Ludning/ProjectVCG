using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MapNode : VisualElement
{
    public void SetTile(TileType type)
    {
        Clear();
        
        if (type == TileType.EMPTY)
            return;
        Sprite tileSprite = MapDataReader.LoadTileSprite(type);
        // 이미지 추가
        var image = new Image();
        image.sprite = tileSprite;
        image.style.width = 40;
        image.style.height = 40;
        image.pickingMode = PickingMode.Ignore;
        Add(image);
    }
}
