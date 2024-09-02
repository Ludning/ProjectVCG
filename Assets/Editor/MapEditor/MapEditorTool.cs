using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MapEditorTool : EditorWindow
{
    [SerializeField] private int m_SelectedIndex = -1;

    private MapDrawSpace _mapDrawSpace;
    private TilePalette _tilePalette;
    private Inspector _inspector;

    private int _defalutWidth = 5;
    private int _defalutHeight = 5;
    
    [MenuItem("Map Editor/Map Editor Tool")]
    public static void ShowMyEditor()
    {
        // menu item을 선택했을 때 EditorWindow 호출
        EditorWindow wnd = GetWindow<MapEditorTool>();
        wnd.titleContent = new GUIContent("Map Editor Tool");

        // window크기 설정
        wnd.minSize = new Vector2(1000, 720);
        wnd.maxSize = new Vector2(1000, 720);
    }
    
    public void CreateGUI()
    {
        VisualElement leftContainer = new VisualElement();
        VisualElement rightContainer = new VisualElement();
        
        // 스타일 설정 (필요에 따라 변경)
        rootVisualElement.style.flexDirection = FlexDirection.Row;
        leftContainer.style.width = Length.Percent(70);
        rightContainer.style.width = Length.Percent(30);
        rightContainer.style.flexDirection = FlexDirection.Column;
        rightContainer.style.flexGrow = 1;
        
        _mapDrawSpace = new MapDrawSpace(_defalutWidth, _defalutHeight);
        _tilePalette = new TilePalette();
        _inspector = new Inspector();
        _tilePalette.style.height = Length.Percent(50);
        _inspector.style.height = Length.Percent(50);
        leftContainer.Add(_mapDrawSpace);
        rightContainer.Add(_tilePalette);
        rightContainer.Add(_inspector);
        rootVisualElement.Add(leftContainer);
        rootVisualElement.Add(rightContainer);

        _mapDrawSpace.SelectedNodeChanged += OnNodeSelectionChange;

        /*//project의 모든 sprites를 list에 저장
        var allObjectGuids = AssetDatabase.FindAssets("t:Sprite");
        var allObjects = new List<Sprite>();
        foreach (var guid in allObjectGuids)
        {
            allObjects.Add(AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(guid)));
        }
        //OnLoadResource();


        // 왼쪽 창을 고정하여 두 개의 창으로 된 보기를 만듭니다.
        //var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);

        // 패널을 루트 요소에 자식으로 추가하여 시각적 트리에 추가합니다.
        //rootVisualElement.Add(splitView);

        //var leftGridPane = new UnityEngine.UIElements.;

        // TwoPaneSplitView에는 항상 두 개의 자식 요소가 필요합니다.
        //ScrollView rightContainer = new ScrollView(ScrollViewMode.Vertical);*/
        /*// 모든 스프라이트의 이름으로 목록 뷰를 초기화합니다.
        //leftPane.makeItem = () => new Label();
        //leftPane.bindItem = (item, index) => { (item as Label).text = Tiles[index].name; };
        //leftPane.itemsSource = Tiles;

        // 사용자의 선택에 반응합니다.
        //leftPane.selectionChanged += OnTileSelectionChange;

        // 핫 새로 고침 이전으로 선택 인덱스를 복원합니다.
        //leftPane.selectedIndex = m_SelectedIndex;

        // 선택 항목이 변경되면 선택 색인을 저장합니다.
        //leftPane.selectionChanged += (items) => { m_SelectedIndex = leftPane.selectedIndex; };*/
    }

    private void OnLoadMapData()
    {
        
    }
    
    private void OnNodeSelectionChange(MapNode selectedNode)
    {
        _inspector.SetInspector(selectedNode);
        /*// Clear all previous content from the pane.
        // 창에서 이전 콘텐츠를 모두 지웁니다.
        m_RightPane.Clear();

        var enumerator = selectedItems.GetEnumerator();
        if (enumerator.MoveNext())
        {
            var selectedSprite = enumerator.Current as Sprite;
            if (selectedSprite != null)
            {
                // Add a new Image control and display the sprite.
                // 새 이미지 컨트롤을 추가하고 스프라이트를 표시합니다.
                var spriteImage = new Image();
                spriteImage.scaleMode = ScaleMode.ScaleToFit;
                spriteImage.sprite = selectedSprite;

                // Add the Image control to the right-hand pane.
                // 오른쪽 창에 이미지 컨트롤을 추가합니다.
                m_RightPane.Add(spriteImage);
            }
        }*/
    }
}