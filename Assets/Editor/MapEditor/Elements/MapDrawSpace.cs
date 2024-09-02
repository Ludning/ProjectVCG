using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapDrawSpace : VisualElement
{
    private MapDrawSpaceNode[,] gridElements; // 2x2 그리드 배열
    private int xSize = 2; // 2x2 그리드 사이즈
    private int ySize = 2; // 2x2 그리드 사이즈
    
    public MapDrawSpaceNode selectedElement; // 선택된 VisualElement
    
    public event Action<MapDrawSpaceNode> SelectedNodeChanged;

    public MapDrawSpace(int x, int y)
    {
        xSize = x;
        ySize = y;
        SetStyle();
        CreateGrid(xSize, ySize);
    }

    public void CreateGrid(int x, int y)
    {
        Clear();
        
        gridElements = new MapDrawSpaceNode[x, y];

        // 그리드의 총 크기와 MapDrawSpace의 크기를 고려하여 중앙 정렬 계산
        float gridWidth = x * 60; // 70은 각 MapNode의 width (50) + margin (20)이라고 가정
        float gridHeight = y * 60; // 70은 각 MapNode의 height (50) + margin (20)이라고 가정
        
        // MapDrawSpace의 중앙에 맞추기 위한 위치 조정
        int startX = (int)((1000 * 0.7 - gridWidth) / 2);
        int startY = (int)((720 - gridHeight) / 2);
        
        for (int col = 0; col < y; col++)
        {
            for (int row = 0; row < x; row++)
            {
                var square = CreateSquareElement(row, col, startX, startY);
                gridElements[row, col] = square;
                Add(square);
            }
        }
    }

    public void LoadGrid(TableData tableData)
    {
        Clear();
        
        xSize = tableData.Size.x;
        ySize = tableData.Size.y;
        
        gridElements = new MapDrawSpaceNode[xSize, ySize];
        
        // 그리드의 총 크기와 MapDrawSpace의 크기를 고려하여 중앙 정렬 계산
        float gridWidth = xSize * 60; // 70은 각 MapNode의 width (50) + margin (20)이라고 가정
        float gridHeight = ySize * 60; // 70은 각 MapNode의 height (50) + margin (20)이라고 가정
        
        // MapDrawSpace의 중앙에 맞추기 위한 위치 조정
        int startX = (int)((1000 * 0.7 - gridWidth) / 2);
        int startY = (int)((720 - gridHeight) / 2);
        
        foreach (var table in tableData.Table)
        {
            
        }
        for (int col = 0; col < ySize; col++)
        {
            for (int row = 0; row < xSize; row++)
            {
                var square = CreateSquareElement(row, col, startX, startY);
                gridElements[row, col] = square;
                Add(square);
            }
        }
    }

    private void SetStyle()
    {
        // Relative 또는 Absolute 위치를 사용하여 자식 요소의 절대 위치를 처리할 수 있도록 설정
        style.position = Position.Relative;
        style.width = Length.Percent(100);
        style.height = Length.Percent(100);
        style.backgroundColor = new Color(1, 0, 0, 0.1f);  // 연한 빨간색
    }

    private MapDrawSpaceNode CreateSquareElement(int x, int y, int startX, int startY)
    {
        var square = new MapDrawSpaceNode(x, y, startX, startY);
        square.RegisterCallback<ClickEvent>(evt => OnNodeSelectionChange(square));
        return square;
    }

    //선택된 Node가 변경되었을 때 호출되는 함수
    private void OnNodeSelectionChange(MapDrawSpaceNode square)
    {
        selectedElement?.UnSelectedNode();
        selectedElement = square;
        selectedElement.OnSelectedNode();
        SelectedNodeChanged?.Invoke(selectedElement);
        Debug.Log("Selected element: " + selectedElement.Node.TileType);
    }

    public TableData GetTableData()
    {
        TableData tableData = new TableData();
        tableData.Size = new Vector2Int(xSize, ySize);
        tableData.Table = new Dictionary<Vector2Int, NodeData>();
        for (int col = 0; col < ySize; col++)
        {
            for (int row = 0; row < xSize; row++)
            {
                tableData.Table.Add(gridElements[row, col].Position, gridElements[row, col].Node);
            }
        }
        return tableData;
    }
}
