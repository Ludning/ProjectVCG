using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TilePalette : VisualElement
{
    private Dictionary<string, TilePaletteNode> gridElements;
    
    public TilePaletteNode selectedElement; // 선택된 VisualElement
    
    public event Action<TilePaletteNode> SelectedNodeChanged;
    
    public TilePalette()
    {
        SetStyle();
        CreateGrid();
    }
    private void SetStyle()
    {
        style.flexGrow = 1;
        style.backgroundColor = new Color(0, 1, 0, 0.1f);  // 연한 초록색
        
        // 스타일링
        style.flexDirection = FlexDirection.Row;
        style.flexWrap = Wrap.Wrap;
        style.width = Length.Percent(100);
        style.height = Length.Percent(100);
    }
    private void CreateGrid()
    {
        Dictionary<string, TileData> tileDataDictionary = DataManager.Instance.GetGameDataDictionary<TileData>();
        gridElements = new Dictionary<string, TilePaletteNode>();

        foreach (var tileData in tileDataDictionary)
        {
            var square = CreateSquareElement(tileData);
            gridElements.Add(tileData.Key, square);
            Add(square);
        }
    }
    private TilePaletteNode CreateSquareElement(KeyValuePair<string, TileData> tile)
    {
        var square = new TilePaletteNode(tile.Value.TileType);
        square.RegisterCallback<ClickEvent>(evt => OnTileSelectionChange(square));
        return square;
    }
    private void OnTileSelectionChange(TilePaletteNode square)
    {
        selectedElement = square;
        Debug.Log("Selected element: " + selectedElement);
        SelectedNodeChanged?.Invoke(selectedElement);
    }
}
