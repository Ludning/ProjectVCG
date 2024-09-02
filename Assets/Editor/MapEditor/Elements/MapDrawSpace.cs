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
        LoadOrCreateGrid(xSize, ySize);
    }

    private void LoadOrCreateGrid(int x, int y)
    {
        gridElements = new MapDrawSpaceNode[x, y];

        // 그리드의 총 크기와 MapDrawSpace의 크기를 고려하여 중앙 정렬 계산
        float gridWidth = x * 70; // 70은 각 MapNode의 width (50) + margin (20)이라고 가정
        float gridHeight = y * 70; // 70은 각 MapNode의 height (50) + margin (20)이라고 가정
        
        // MapDrawSpace의 중앙에 맞추기 위한 위치 조정
        int startX = (int)((1000 * 0.7 - gridWidth) / 2);
        int startY = (int)((720 - gridHeight) / 2);
        
        for (int row = 0; row < y; row++)
        {
            for (int col = 0; col < x; col++)
            {
                var square = CreateSquareElement(row, col, startX, startY);
                gridElements[col, row] = square;
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
    
    // VisualElement에 이미지를 설정하는 메서드
    /*public void SetImage(int row, int col, Texture2D texture)
    {
        if (row >= 0 && row < gridSize && col >= 0 && col < gridSize)
        {
            var image = gridElements[row, col].Q<Image>();
            if (image != null)
            {
                image.image = texture;
            }
        }
    }*/

    //선택된 Node가 변경되었을 때 호출되는 함수
    private void OnNodeSelectionChange(MapDrawSpaceNode square)
    {
        selectedElement?.UnSelectedNode();
        selectedElement = square;
        selectedElement.OnSelectedNode();
        Debug.Log("Selected element: " + selectedElement);
        //선택된 Element의 강조 추가
        //기존에 선택된 Element의 강조 제거
        /*// 기존에 선택된 Element의 강조 제거
        if (selectedElement != null)
        {
            //selectedElement.style.borderColor = Color.black;
        }

        // 새로 선택된 Element의 강조 추가
        //selectedElement = square;
        //selectedElement.style.borderColor = Color.red;

        // 선택된 VisualElement를 반환하는 로직 추가 가능
        // 예: Debug.Log("Selected element: " + selectedElement);*/
    }
}
