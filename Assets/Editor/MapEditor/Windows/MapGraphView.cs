using DS.Utilities;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class MapGraphView : GraphView
{
    private MapEditorWindow editorWindow;
    //private DSSearchWindow searchWindow;

    //private SerializableDictionary<string, DSNodeErrorData> ungroupedNodes;
    //private SerializableDictionary<string, DSGroupErrorData> groups;
    //private SerializableDictionary<Group, SerializableDictionary<string, DSNodeErrorData>> groupedNodes;

    private int nameErrorsAmount;

    public int NameErrorsAmount
    {
        get
        {
            return nameErrorsAmount;
        }

        set
        {
            nameErrorsAmount = value;

            if (nameErrorsAmount == 0)
            {
                editorWindow.EnableSaving();
            }

            if (nameErrorsAmount == 1)
            {
                editorWindow.DisableSaving();
            }
        }
    }

    public MapGraphView(MapEditorWindow mapEditorWindow)
    {
        editorWindow = mapEditorWindow;

        //ungroupedNodes = new SerializableDictionary<string, DSNodeErrorData>();
        //groups = new SerializableDictionary<string, DSGroupErrorData>();
        //groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, DSNodeErrorData>>();

        //AddManipulators();
        
        //그리드 배경화면 추가
        AddGridBackground();

        //삭제된 요소에 대하여
        //OnElementsDeleted();
        
        //OnGroupElementsAdded();
        //OnGroupElementsRemoved();
        //OnGroupRenamed();
        //OnGraphViewChanged();

        //AddStyles();
    }
    //그리드 배경화면 추가
    private void AddGridBackground()
    {
        GridBackground gridBackground = new GridBackground();

        gridBackground.StretchToParentSize();

        Insert(0, gridBackground);
    }
    


    public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
    {
        Vector2 worldMousePosition = mousePosition;

        if (isSearchWindow)
        {
            worldMousePosition = editorWindow.rootVisualElement.ChangeCoordinatesTo(
                editorWindow.rootVisualElement.parent, mousePosition - editorWindow.position.position);
        }

        Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

        return localMousePosition;
    }

    public void ClearGraph()
    {
        graphElements.ForEach(graphElement => RemoveElement(graphElement));

        //groups.Clear();
        //groupedNodes.Clear();
        //ungroupedNodes.Clear();

        NameErrorsAmount = 0;
    }

}