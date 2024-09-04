using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Inspector : VisualElement
{
    private EnumField TileTypeDropdown;
    private Toggle IsPlayerPositionToggle;
    private EnumField PlayerDirectionDropdown;
    private Toggle SpawnObjectToggle;
    private EnumField SpawnObjectTypeDropdown;

    private TableData tableData;
    
    public Inspector()
    {
        SetStyle();
        SetInspector(null, null);
    }

    private void SetStyle()
    {
        style.flexGrow = 1;
        style.backgroundColor = new Color(0, 0, 1, 0.1f); // 연한 파란색
    }

    public void SetInspector(TableData tableData, MapDrawSpaceNode mapNode)
    {
        Clear();

        if (tableData == null)
            return;
        if (mapNode == null)
            return;

        this.tableData = tableData;
        
        Debug.Log(mapNode.Node.TileType);
        Debug.Log(mapNode.Node.TileType);
        Debug.Log(mapNode.Node.IsSpawnObject);
        Debug.Log(mapNode.Node.SpawnObjectType);
        TileTypeDropdown = new EnumField("바닥 유형", mapNode.Node.TileType);
        IsPlayerPositionToggle = new Toggle("플레이어 시작 위치");
        PlayerDirectionDropdown = new EnumField("플레이어 방향", tableData.PlayerDirection);
        SpawnObjectToggle = new Toggle("아이템 스폰");
        SpawnObjectTypeDropdown = new EnumField("아이템 스폰 유형", mapNode.Node.SpawnObjectType);
        
        // TileType 드롭다운
        TileTypeDropdown.Init(mapNode.Node.TileType);
        TileTypeDropdown.RegisterValueChangedCallback(evt => OnTileTypeChanged(evt, mapNode));
        TileTypeDropdown.SetEnabled(false);
        Add(TileTypeDropdown);

        // IsPlayerPosition 체크박스
        IsPlayerPositionToggle.value = (tableData.PlayerPosition == mapNode.Position) ? true : false;
        IsPlayerPositionToggle.RegisterValueChangedCallback(evt =>
            OnIsPlayerPositionChanged(evt, mapNode));
        Add(IsPlayerPositionToggle);

        // PlayerDirection 드롭다운
        PlayerDirectionDropdown.Init(tableData.PlayerDirection);
        PlayerDirectionDropdown.RegisterValueChangedCallback(evt => OnPlayerDirectionChanged(evt, mapNode));
        PlayerDirectionDropdown.SetEnabled(IsPlayerPositionToggle.value); // 초기 상태 설정
        Add(PlayerDirectionDropdown);

        // SpawnObject 체크박스
        SpawnObjectToggle.value = mapNode.Node.IsSpawnObject;
        SpawnObjectToggle.RegisterValueChangedCallback(evt =>
            OnSpawnObjectChanged(evt, mapNode));
        Add(SpawnObjectToggle);

        // SpawnObjectType 드롭다운
        SpawnObjectTypeDropdown.Init(mapNode.Node.SpawnObjectType);
        SpawnObjectTypeDropdown.RegisterValueChangedCallback(evt => OnSpawnObjectTypeChanged(evt, mapNode));
        SpawnObjectTypeDropdown.SetEnabled(mapNode.Node.IsSpawnObject); // 초기 상태 설정
        Add(SpawnObjectTypeDropdown);
        
        // TileType이 Empty일 경우 요소 비활성화
        if (mapNode.Node.TileType == TileType.EMPTY)
        {
            IsPlayerPositionToggle.SetEnabled(false);
            PlayerDirectionDropdown.SetEnabled(false);
            SpawnObjectToggle.SetEnabled(false);
            SpawnObjectTypeDropdown.SetEnabled(false);
        }
    }

    // 외부로 분리한 콜백 함수들
    private void OnTileTypeChanged(ChangeEvent<Enum> evt, MapDrawSpaceNode mapNode)
    {
        mapNode.Node.TileType = (TileType)evt.newValue;
        
        // TileType 변경 후 비활성화 처리
        bool isEnabled = mapNode.Node.TileType != TileType.EMPTY;
        IsPlayerPositionToggle.SetEnabled(isEnabled);
        PlayerDirectionDropdown.SetEnabled(isEnabled && IsPlayerPositionToggle.value);
        SpawnObjectToggle.SetEnabled(isEnabled);
        SpawnObjectTypeDropdown.SetEnabled(isEnabled && mapNode.Node.IsSpawnObject);
    }

    private void OnIsPlayerPositionChanged(ChangeEvent<bool> evt, MapDrawSpaceNode mapNode)
    {
        if (evt.newValue == true)
        {
            tableData.PlayerPosition = mapNode.Position;
            PlayerDirectionDropdown.SetEnabled(evt.newValue);
        }
    }

    private void OnPlayerDirectionChanged(ChangeEvent<Enum> evt, MapDrawSpaceNode mapNode)
    {
        tableData.PlayerDirection = (Direction)evt.newValue;
    }

    private void OnSpawnObjectChanged(ChangeEvent<bool> evt, MapDrawSpaceNode mapNode)
    {
        mapNode.Node.IsSpawnObject = evt.newValue;
        SpawnObjectTypeDropdown.SetEnabled(evt.newValue);
    }

    private void OnSpawnObjectTypeChanged(ChangeEvent<Enum> evt, MapDrawSpaceNode mapNode)
    {
        mapNode.Node.SpawnObjectType = (ItemType)evt.newValue;
    }
}
