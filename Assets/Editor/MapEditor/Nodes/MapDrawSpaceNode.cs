using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapDrawSpaceNode : MapNode
{
    
    public MapDrawSpaceNode(int x, int y, int startX, int startY)
    {
        SetStyle(x, y, startX, startY);
        TileType type = MapDataReader.LoadTileType(x, y);
        SetTile(type);
    }
    private void SetStyle(int x, int y, int startX, int startY)
    {
        Debug.Log($"{x}, {y}, {startX}, {startY}");
        style.width = 50;
        style.height = 50;
        
        // Absolute 위치 설정
        style.position = Position.Absolute;
        style.left = startY + y * 70;
        style.top = startX + x * 70;
        
        style.alignItems = Align.Center;         // 수평 중앙 정렬
        style.justifyContent = Justify.Center;   // 수직 중앙 정렬

        style.backgroundColor = new Color(0, 0.5f, 1, 0.2f);  // 연한 파란색
    }

    public void OnSelectedNode()
    {
        style.backgroundColor = new Color(1, 0, 1, 0.2f);  // 연한 빨간색
    }
    public void UnSelectedNode()
    {
        style.backgroundColor = new Color(0, 0.5f, 1, 0.2f);  // 연한 파란색
    }
}
