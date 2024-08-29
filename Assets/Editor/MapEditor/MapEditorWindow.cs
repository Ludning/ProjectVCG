using DS.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MapEditorWindow : EditorWindow
{
    //그래프 뷰
    private MapGraphView graphView;

    //기본 파일 이름
    private readonly string mapFileName = "DialoguesFileName";

    //텍스트 필드
    private static TextField ChapterTextField;
    private static TextField StageTextField;
    
    //저장 버튼
    private Button saveButton;
    
    [MenuItem("Map Editor/Map Editor Window")]
    public static void Open()
    {
        GetWindow<MapEditorWindow>("Map Editor");
    }

    //활성화 시 호출
    private void OnEnable()
    {
        AddGraphView();
        AddToolbar();

        //AddStyles();
    }
    //그래프 뷰 추가
    private void AddGraphView()
    {
        //그래프 뷰 생성
        graphView = new MapGraphView(this);

        //그래프 뷰 사이즈 확장
        graphView.StretchToParentSize();

        //루트 VisualElement에 그래프 뷰 추가
        rootVisualElement.Add(graphView);
    }

    //툴바 추가
    private void AddToolbar()
    {
        //툴바 생성
        UnityEditor.UIElements.Toolbar toolbar = new UnityEditor.UIElements.Toolbar();

        //TextField 생성
        //fileNameTextField = UI_ElementUtility.CreateTextField(mapFileName, "File Name:",
        //    callback => { fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters(); });

        //저장버튼 생성, 이벤트 연결
        saveButton = UI_ElementUtility.CreateButton("Save", () => Save());

        //로드버튼 생성, 이벤트 연결
        Button loadButton = UI_ElementUtility.CreateButton("Load", () => Load());
        //클리어버튼 생성, 이벤트 연결
        Button clearButton = UI_ElementUtility.CreateButton("Clear", () => Clear());
        //리셋버튼 생성, 이벤트 연결
        Button resetButton = UI_ElementUtility.CreateButton("Reset", () => ResetGraph());


        //toolbar.Add(fileNameTextField);
        toolbar.Add(saveButton);
        toolbar.Add(loadButton);
        toolbar.Add(clearButton);
        toolbar.Add(resetButton);

        //toolbar.AddStyleSheets("DialogueSystem/DSToolbarStyles.uss");

        rootVisualElement.Add(toolbar);
    }

    //스타일 추가
    /*private void AddStyles()
    {
        rootVisualElement.AddStyleSheets("DialogueSystem/DSVariables.uss");
    }*/

    //저장 기능
    private void Save()
    {
        if (string.IsNullOrEmpty(ChapterTextField.value))
        {
            EditorUtility.DisplayDialog("Invalid file name.", "Please ensure the file name you've typed in is valid.",
                "Roger!");

            return;
        }

        //UI_IOUtility.Initialize(graphView, ChapterTextField.value);
        //UI_IOUtility.Save();
    }

    //로드 기능
    private void Load()
    {
        string filePath =
            EditorUtility.OpenFilePanel("Dialogue Graphs", "Assets/Editor/DialogueSystem/Graphs", "asset");

        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        Clear();

        //UI_IOUtility.Initialize(graphView, Path.GetFileNameWithoutExtension(filePath));
        //UI_IOUtility.Load();
    }

    //클리어 기능
    private void Clear()
    {
        graphView.ClearGraph();
    }

    //그래프 초기화 기능
    private void ResetGraph()
    {
        Clear();

        //UpdateChapterIndex(mapFileName);
        //UpdateStageIndex(mapFileName);
    }

    //챕터 수정기능
    public static void UpdateChapterIndex(string ChapterIndex)
    {
        ChapterTextField.value = ChapterIndex;
    }
    //스테이지 수정기능
    public static void UpdateStageIndex(string StageIndex)
    {
        ChapterTextField.value = StageIndex;
    }

    //저장기능 활성화 기능
    public void EnableSaving()
    {
        saveButton.SetEnabled(true);
    }

    //저장기능 비활성화 기능
    public void DisableSaving()
    {
        saveButton.SetEnabled(false);
    }
}
