using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MapEditorTool : EditorWindow
{
    [SerializeField] private int m_SelectedIndex = -1;

    private MapInfoListView _mapInfoListView;
    private MapDrawSpace _mapDrawSpace;
    private TilePalette _tilePalette;
    private Inspector _inspector;

    private TableData tableData;

    private int _width = 5;
    private int _height = 5;
    
    private PopupField<string> _stageDropdown;

    private string _currentStage = "3000";
    
    [MenuItem("Map Editor/Map Editor Tool")]
    public static void ShowMyEditor()
    {
        // menu item을 선택했을 때 EditorWindow 호출
        EditorWindow wnd = GetWindow<MapEditorTool>();
        wnd.titleContent = new GUIContent("Map Editor Tool");

        // window크기 설정
        wnd.minSize = new Vector2(1600, 720);
        wnd.maxSize = new Vector2(1600, 720);
    }
    
    public void CreateGUI()
    {
        VisualElement toolbarContainer = new VisualElement();   // 상단 메뉴바를 포함할 컨테이너
        VisualElement toolbar = new VisualElement();    // 상단 메뉴바 생성
        TextField mapSizeXField = new TextField("X");
        TextField mapSizeYField = new TextField("Y");
        Button initButton = new Button(OnInitMapData) { text = "Init" };    // 맵 크기 적용 버튼
        Button saveButton = new Button(OnSaveMapData) { text = "Save" };    // 저장 버튼
        Button loadButton = new Button(() => OnLoadMapData(_stageDropdown.value)) { text = "Load" };    // 로드 버튼
        VisualElement mainContainer = new VisualElement();  // 나머지 UI 요소들을 담을 컨테이너
        VisualElement leftContainer = new VisualElement();
        VisualElement centerContainer = new VisualElement();
        VisualElement rightContainer = new VisualElement();
        RowItem header = new RowItem(true);
        
        var stageDataDictionary = DataManager.Instance.GetGameDataDictionary<StageData>();
        _stageDropdown = new PopupField<string>("Select Stage", stageDataDictionary.Keys.ToList(), 0);  // 스테이지 선택 드롭다운

        _mapInfoListView = new MapInfoListView();
        _mapDrawSpace = new MapDrawSpace(_currentStage, _width, _height);
        _tilePalette = new TilePalette();
        _inspector = new Inspector();
        
        mapSizeXField.value = _width.ToString();
        mapSizeXField.RegisterValueChangedCallback(evt => 
        { 
            _width = int.Parse(evt.newValue);
            mapSizeXField.value = evt.newValue.ToString();
        });
        mapSizeYField.value = _height.ToString();
        mapSizeYField.RegisterValueChangedCallback(evt => 
        { 
            _height = int.Parse(evt.newValue);
            mapSizeYField.value = evt.newValue.ToString();
        });
        
        _stageDropdown.RegisterValueChangedCallback(evt => OnStageChanged(evt.newValue));
        
        header.Position.text = "위치";
        header.TileType.text = "타일 종류";
        header.CookingPropertyType.text = "타일 속성";
        header.TileId.text = "타일 아이디";
        header.SpawnObject.text = "스폰될 아이템";
        header.SpawnNpc.text = "스폰될 Npc";
        header.PlayerPosition.text = "플레이어";
        
        
        // 스타일 설정 (필요에 따라 변경)
        toolbarContainer.style.flexDirection = FlexDirection.Column;
        toolbar.style.flexDirection = FlexDirection.Row;
        mapSizeXField.style.width = 200;
        mapSizeYField.style.width = 200;
        mainContainer.style.flexDirection = FlexDirection.Row;
        mainContainer.style.flexGrow = 1; // 남은 공간을 모두 차지하도록 설정
        _stageDropdown.style.flexGrow = 1;
        rootVisualElement.style.flexDirection = FlexDirection.Column;
        leftContainer.style.width = Length.Percent(30);
        centerContainer.style.width = Length.Percent(50);
        rightContainer.style.width = Length.Percent(20);
        rightContainer.style.flexDirection = FlexDirection.Column;
        rightContainer.style.flexGrow = 1;
        _tilePalette.style.height = Length.Percent(50);
        _inspector.style.height = Length.Percent(50);
        
        //계층 구조 설정
        toolbar.Add(mapSizeXField);
        toolbar.Add(mapSizeYField);
        toolbar.Add(initButton);
        toolbar.Add(_stageDropdown);
        toolbar.Add(saveButton);
        toolbar.Add(loadButton);
        toolbarContainer.Add(toolbar);  // 메뉴바를 컨테이너에 추가
        
        leftContainer.Add(header);
        leftContainer.Add(_mapInfoListView);
        centerContainer.Add(_mapDrawSpace);
        rightContainer.Add(_tilePalette);
        rightContainer.Add(_inspector);
        
        mainContainer.Add(leftContainer);
        mainContainer.Add(centerContainer);
        mainContainer.Add(rightContainer);

        // 최상위 컨테이너에 메뉴바와 나머지 UI 요소들을 추가
        rootVisualElement.Add(toolbarContainer);
        rootVisualElement.Add(mainContainer);

        _mapDrawSpace.SelectedNodeChanged += OnNodeSelectionChange;
        _tilePalette.SelectedNodeChanged += OnClickTilePallete;
        _mapInfoListView.SelectedListItemChanged += OnListSelectionChange;
    }
    private void OnLoadMapData(string stageName)
    {
        tableData = DataManager.Instance.GetTableData(stageName);
        if (tableData != null)
            _mapDrawSpace.LoadGrid(tableData);
        else
            _mapDrawSpace.CreateGrid(5, 5);
        _mapInfoListView.OnDataChanged(tableData);
    }

    private void OnSaveMapData()
    {
        Debug.Log("OnSaveMapData");
        MapData mapData = MapDataReader.LoadMapData();
        
        if (mapData == null)
            mapData = new MapData();
        if (mapData.TableData == null)
            mapData.TableData = new Dictionary<string, TableData>();

        mapData.TableData[_currentStage] = _mapDrawSpace.GetTableData();
        mapData.TableData[_currentStage].PlayerPosition = tableData.PlayerPosition;
        mapData.TableData[_currentStage].PlayerDirection = tableData.PlayerDirection;
        mapData.TableData[_currentStage].LevelDataDictionary = tableData.LevelDataDictionary;
        MapDataReader.SaveMapData(mapData);
        _mapInfoListView.OnDataChanged(tableData);
    }
    
    private void OnInitMapData()
    {
        _mapDrawSpace.CreateGrid(_width, _height);
    }
    
    private void OnStageChanged(string stage)
    {
        _currentStage = stage;
        OnLoadMapData(_currentStage);
        _mapInfoListView.OnDataChanged(tableData);
    }
    
    private void OnNodeSelectionChange(MapDrawSpaceNode selectedNode)
    {
        _inspector.SetInspector(tableData, selectedNode);
    }
    private void OnClickTilePallete(TilePaletteNode selectedNode)
    {
        _mapDrawSpace.selectedElement.SetNode(selectedNode.TileType);
        _mapDrawSpace.selectedElement.SetTile(selectedNode.TileType);
        _inspector.SetInspector(tableData, _mapDrawSpace.selectedElement);
    }
    private void OnListSelectionChange(int index)
    {
        _mapDrawSpace.SetNodeSelectionChange(tableData.Size, index);
        _inspector.SetInspector(tableData, _mapDrawSpace.selectedElement);
    }
}