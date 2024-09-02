using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapDrawSpaceNode : MapNode
{
    public Vector2Int Position;
    public NodeData Node;
    public MapDrawSpaceNode(int x, int y, int startX, int startY)
    {
        SetStyle(x, y, startX, startY);
        TileType type = MapDataReader.LoadTileType(x, y);
        Position = new Vector2Int(x, y);
        SetNode(type);
        SetTile(type);
    }
    public void SetNode(TileType type)
    {
        switch (type)
        {
            case TileType.EMPTY:
                Node = null;
                break;
            default:
                Node = new NodeData();
                Node.TileType = type;
                break;
        }
    }
    private void SetStyle(int x, int y, int startX, int startY)
    {
        Debug.Log($"{x}, {y}, {startX}, {startY}");
        style.width = 50;
        style.height = 50;
        
        // Absolute 위치 설정
        style.position = UnityEngine.UIElements.Position.Absolute;
        style.left = startY + y * 60;
        style.top = startX + x * 60;
        
        style.alignItems = Align.Center;         // 수평 중앙 정렬
        style.justifyContent = Justify.Center;   // 수직 중앙 정렬

        style.backgroundColor = new Color(0, 0.5f, 1, 0.2f);  // 연한 파란색
    }

    public void OnSelectedNode()
    {
        style.backgroundColor = new Color(1, 0, 0, 1);  // 연한 빨간색
    }
    public void UnSelectedNode()
    {
        style.backgroundColor = new Color(0, 0.5f, 1, 0.2f);  // 연한 파란색
    }
}
