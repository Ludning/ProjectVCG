using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Inspector : VisualElement
{
    //CUSTOMER
    
    private EnumField TileTypeDropdown;
    private TextField TileKeyField;
    //private EnumField TileDetailTypeDropdown;
    private EnumField CookingPropertyTypeDropdown;
    private Toggle IsPlayerPositionToggle;
    private EnumField PlayerDirectionDropdown;
    private Toggle IsSpawnNpcToggle;
    private EnumField SpawnNpcTypeDropdown;
    private Toggle IsSpawnObjectToggle;
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
        Debug.Log(mapNode.Node.IsSpawnObject);
        Debug.Log(mapNode.Node.SpawnObjectType);
        
        TileTypeDropdown = new EnumField("바닥 유형", mapNode.Node.TileType);
        TileKeyField = new TextField("타일 고유번호");
        //TileDetailTypeDropdown = new EnumField("바닥 속성", mapNode.Node.TileDetailType);
        CookingPropertyTypeDropdown = new EnumField("타일 속성", mapNode.Node.CookingPropertyType);
        IsPlayerPositionToggle = new Toggle("플레이어 시작 위치");
        PlayerDirectionDropdown = new EnumField("플레이어 방향", tableData.PlayerDirection);
        
        IsSpawnNpcToggle = new Toggle("NPC 스폰");
        SpawnNpcTypeDropdown = new EnumField("NPC 스폰 유형", mapNode.Node.SpawnNpcType);
        IsSpawnObjectToggle = new Toggle("아이템 스폰");
        SpawnObjectTypeDropdown = new EnumField("아이템 스폰 유형", mapNode.Node.SpawnObjectType);
        
        // TileType 드롭다운
        TileTypeDropdown.Init(mapNode.Node.TileType);
        TileTypeDropdown.RegisterValueChangedCallback(evt => OnTileTypeChanged(evt, mapNode));
        TileTypeDropdown.SetEnabled(false);
        Add(TileTypeDropdown);
        
        // TileKeyField 텍스트 박스
        int nodePosition = Vector2IntConverter.Vec2ToInt(tableData.Size, mapNode.Position);
        if(tableData.LevelDataDictionary != null)
            TileKeyField.value = tableData.LevelDataDictionary.GetValueOrDefault(nodePosition, "");
        TileKeyField.RegisterValueChangedCallback(evt => OnTileKeyChanged(evt, mapNode));
        Add(TileKeyField);
        
        // TileType 드롭다운
        //TileDetailTypeDropdown.Init(mapNode.Node.TileDetailType);
        //TileDetailTypeDropdown.RegisterValueChangedCallback(evt => OnTileTypeChanged(evt, mapNode));
        //Add(TileDetailTypeDropdown);
        
        // CookingPropertyType 드롭다운
        CookingPropertyTypeDropdown.Init(mapNode.Node.CookingPropertyType);
        CookingPropertyTypeDropdown.RegisterValueChangedCallback(evt => OnCookingPropertyTypeChanged(evt, mapNode));
        Add(CookingPropertyTypeDropdown);

        // IsPlayerPosition 체크박스
        IsPlayerPositionToggle.value = (tableData.PlayerPosition == mapNode.Position) ? true : false;
        IsPlayerPositionToggle.RegisterValueChangedCallback(evt => OnIsPlayerPositionChanged(evt, mapNode));
        Add(IsPlayerPositionToggle);

        // PlayerDirection 드롭다운
        PlayerDirectionDropdown.Init(tableData.PlayerDirection);
        PlayerDirectionDropdown.RegisterValueChangedCallback(evt => OnPlayerDirectionChanged(evt, mapNode));
        PlayerDirectionDropdown.SetEnabled(IsPlayerPositionToggle.value); // 초기 상태 설정
        Add(PlayerDirectionDropdown);
        
        // SpawnNpc 체크박스
        IsSpawnNpcToggle.value = mapNode.Node.IsSpawnNPC;
        IsSpawnNpcToggle.RegisterValueChangedCallback(evt => OnIsSpawnNpcChanged(evt, mapNode));
        Add(IsSpawnNpcToggle);
        
        // SpawnNpcType 드롭다운
        SpawnNpcTypeDropdown.Init(mapNode.Node.SpawnNpcType);
        SpawnNpcTypeDropdown.RegisterValueChangedCallback(evt => OnSpawnNpcTypeChanged(evt, mapNode));
        SpawnNpcTypeDropdown.SetEnabled(mapNode.Node.IsSpawnNPC); // 초기 상태 설정
        Add(SpawnNpcTypeDropdown);

        // SpawnObject 체크박스
        IsSpawnObjectToggle.value = mapNode.Node.IsSpawnObject;
        IsSpawnObjectToggle.RegisterValueChangedCallback(evt => OnIsSpawnObjectChanged(evt, mapNode));
        Add(IsSpawnObjectToggle);

        // SpawnObjectType 드롭다운
        SpawnObjectTypeDropdown.Init(mapNode.Node.SpawnObjectType);
        SpawnObjectTypeDropdown.RegisterValueChangedCallback(evt => OnSpawnObjectTypeChanged(evt, mapNode));
        SpawnObjectTypeDropdown.SetEnabled(mapNode.Node.IsSpawnObject); // 초기 상태 설정
        Add(SpawnObjectTypeDropdown);
        
        
        // TileType이 Empty일 경우 요소 비활성화
        if (mapNode.Node.TileType == TileType.EMPTY)
        {
            //TileDetailTypeDropdown.SetEnabled(false);
            IsPlayerPositionToggle.SetEnabled(false);
            TileKeyField.SetEnabled(false);
            PlayerDirectionDropdown.SetEnabled(false);
            IsSpawnObjectToggle.SetEnabled(false);
            SpawnObjectTypeDropdown.SetEnabled(false);
        }
        if (mapNode.Node.TileType == TileType.COOKING)
            CookingPropertyTypeDropdown.SetEnabled(true);
        else
            CookingPropertyTypeDropdown.SetEnabled(false);

        /*if (mapNode.Node.TileType == TileType.CUSTOMER)
            SpawnNpcTypeDropdown.SetEnabled(true);
        else
            SpawnNpcTypeDropdown.SetEnabled(false);

        if (mapNode.Node.TileType == TileType.INGREDIENT)
            SpawnObjectTypeDropdown.SetEnabled(true);
        else
            SpawnObjectTypeDropdown.SetEnabled(false);*/
    }

    // 외부로 분리한 콜백 함수들
    private void OnTileTypeChanged(ChangeEvent<Enum> evt, MapDrawSpaceNode mapNode)
    {
        mapNode.Node.TileType = (TileType)evt.newValue;
        
        // TileType 변경 후 비활성화 처리
        bool isEnabled = mapNode.Node.TileType != TileType.EMPTY;
        IsPlayerPositionToggle.SetEnabled(isEnabled);
        PlayerDirectionDropdown.SetEnabled(isEnabled && IsPlayerPositionToggle.value);
        IsSpawnObjectToggle.SetEnabled(isEnabled);
        
        //CUSTOMER
        SpawnNpcTypeDropdown.SetEnabled(isEnabled && mapNode.Node.IsSpawnObject);
        SpawnObjectTypeDropdown.SetEnabled(isEnabled && mapNode.Node.IsSpawnObject);

        
        bool isCookingEnabled = mapNode.Node.TileType == TileType.COOKING;
        if (isCookingEnabled == false)
            mapNode.Node.CookingPropertyType = CookingPropertyType.NULL;
        else
            CookingPropertyTypeDropdown.SetEnabled(mapNode.Node.TileType == TileType.COOKING);
    }
    private void OnCookingPropertyTypeChanged(ChangeEvent<Enum> evt, MapDrawSpaceNode mapNode)
    {
        mapNode.Node.CookingPropertyType = (CookingPropertyType)evt.newValue;
    }
    private void OnTileKeyChanged(ChangeEvent<string> evt, MapDrawSpaceNode mapNode)
    {
        int nodePosition = Vector2IntConverter.Vec2ToInt(tableData.Size, mapNode.Position);
        if (tableData.LevelDataDictionary == null)
            tableData.LevelDataDictionary = new Dictionary<int, string>();
        
        if (string.IsNullOrWhiteSpace(evt.newValue))
            tableData.LevelDataDictionary.Remove(nodePosition);
        else
            tableData.LevelDataDictionary[nodePosition] = evt.newValue;
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

    private void OnIsSpawnNpcChanged(ChangeEvent<bool> evt, MapDrawSpaceNode mapNode)
    {
        mapNode.Node.IsSpawnNPC = evt.newValue;
        SpawnNpcTypeDropdown.SetEnabled(evt.newValue);
    }
    
    private void OnSpawnNpcTypeChanged(ChangeEvent<Enum> evt, MapDrawSpaceNode mapNode)
    {
        mapNode.Node.SpawnNpcType = (NpcType)evt.newValue;
    }
    
    private void OnIsSpawnObjectChanged(ChangeEvent<bool> evt, MapDrawSpaceNode mapNode)
    {
        mapNode.Node.IsSpawnObject = evt.newValue;
        SpawnObjectTypeDropdown.SetEnabled(evt.newValue);
    }

    private void OnSpawnObjectTypeChanged(ChangeEvent<Enum> evt, MapDrawSpaceNode mapNode)
    {
        mapNode.Node.SpawnObjectType = (ItemType)evt.newValue;
    }
}
