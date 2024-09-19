using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum LabelType
{
    Position,
    TileType,
    TileDetail,
    TileId,
    SpawnNpc,
    SpawnObject,
    PlayerPosition,
}
public class RowItem : VisualElement
{
    public Label Position;
    public Label TileType;
    public Label CookingPropertyType;
    public Label TileId;
    public Label SpawnNpc;
    public Label SpawnObject;
    public Label PlayerPosition;
    
    private bool _isSelected;

    public RowItem(bool isHeader = false)
    {
        Position = new Label();
        TileType = new Label();
        CookingPropertyType = new Label();
        TileId = new Label();
        SpawnNpc = new Label();
        SpawnObject = new Label();
        PlayerPosition = new Label();
        
        SetStyle(isHeader);
        
        // RowItem에 레이블 추가
        Add(Position);
        Add(TileType);
        Add(CookingPropertyType);
        Add(TileId);
        Add(SpawnNpc);
        Add(SpawnObject);
        Add(PlayerPosition);
    }

    private void SetStyle(bool isHeader = false)
    {
        // RowItem이 가로로 배치되고, 요소들이 오른쪽으로 정렬되도록 설정
        style.flexDirection = FlexDirection.Row;
        style.justifyContent = Justify.FlexStart;
        style.paddingBottom = new StyleLength(1);
        
        SetLabelStyle(Position, LabelType.Position, isHeader);
        SetLabelStyle(TileType, LabelType.TileType, isHeader);
        SetLabelStyle(CookingPropertyType, LabelType.TileDetail, isHeader);
        SetLabelStyle(TileId, LabelType.TileId, isHeader);
        SetLabelStyle(SpawnNpc, LabelType.SpawnNpc, isHeader);
        SetLabelStyle(SpawnObject, LabelType.SpawnObject, isHeader);
        SetLabelStyle(PlayerPosition, LabelType.PlayerPosition, isHeader);
    }
    private void SetLabelStyle(Label label, LabelType type, bool isHeader = false)
    {
        pickingMode = PickingMode.Ignore;
        label.style.flexGrow = 1;
        label.style.unityTextAlign = TextAnchor.MiddleCenter;  // 텍스트 가운데 정렬
        label.style.borderRightColor = Color.black;  // 테두리 설정
        label.style.borderRightWidth = 1;
        label.style.borderBottomWidth = 1;
        label.style.color = Color.black;            // 텍스트 색상
        if (isHeader)
            label.style.backgroundColor = Color.yellow;

        switch (type)
        {
            case LabelType.Position:
                label.style.minWidth = 40;
                label.style.maxWidth = 40;
                break;
            case LabelType.TileType:
                label.style.minWidth = 50;
                label.style.maxWidth = 50;
                break;
            case LabelType.TileDetail:
                label.style.minWidth = 60;
                label.style.maxWidth = 60;
                break;
            case LabelType.TileId:
                label.style.minWidth = 70;
                label.style.maxWidth = 70;
                break;
            case LabelType.SpawnNpc:
                label.style.minWidth = 110;
                label.style.maxWidth = 110;
                break;
            case LabelType.SpawnObject:
                label.style.minWidth = 120;
                label.style.maxWidth = 120;
                break;
            case LabelType.PlayerPosition:
                label.style.minWidth = 30;
                label.style.maxWidth = 30;
                break;
        }
    }
    public void SetSelected(bool isSelected)
    {
        _isSelected = isSelected;
        style.backgroundColor = isSelected ? Color.green : Color.white;
    }
}
