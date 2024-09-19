using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;

public class MapInfoListView : ListView
{
    private List<RowData> _rowDatas;
    
    public event Action<int> SelectedListItemChanged;
    
    public MapInfoListView()
    {
        SetStyle();
        Init();
    }
    
    private void SetStyle()
    {
        style.flexGrow = 1;
        style.backgroundColor = Color.gray;
    }
    private void Init()
    {
        Clear();
        
        _rowDatas = new List<RowData>();
        
        Func<RowItem> makeItem = () => new RowItem();
        Action<VisualElement, int> bindItem = (visualElement, index) =>
        {
            var rowItem = visualElement as RowItem;
            (visualElement as RowItem).Position.text = _rowDatas[index].Position;
            (visualElement as RowItem).TileType.text = _rowDatas[index].TileType;
            (visualElement as RowItem).CookingPropertyType.text = (_rowDatas[index].CookingPropertyType != "NULL") ? _rowDatas[index].CookingPropertyType : "";
            (visualElement as RowItem).TileId.text = _rowDatas[index].TileId;
            (visualElement as RowItem).SpawnNpc.text = _rowDatas[index].SpawnNpc;
            (visualElement as RowItem).SpawnObject.text = _rowDatas[index].SpawnObject;
            (visualElement as RowItem).PlayerPosition.text = (_rowDatas[index].PlayerPosition) ? "TRUE" : "";
            
            
            rowItem.SetSelected(IsItemSelected(index));
        };
        
        fixedItemHeight = 16;
        itemsSource = _rowDatas;
        this.makeItem = makeItem;
        this.bindItem = bindItem;
        
        selectionType = SelectionType.Single;
        
        itemsChosen += objects => Debug.Log(objects);
        selectionChanged += OnSelectionChanged;
        
        Rebuild();
    }
    private bool IsItemSelected(int index)
    {
        // Check if the item at 'index' is selected
        return selectedIndices.Contains(index);
    }
    private void OnSelectionChanged(IEnumerable<object> selectedObjects)
    {
        foreach (var obj in selectedObjects)
        {
            if (obj is RowData rowData)
            {
                Debug.Log($"선택된 RowData: Index={rowData.positionIndex}");
                SelectedListItemChanged?.Invoke(rowData.positionIndex);
            }
        }
        Rebuild();
    }
    /*private void OnSelectionChanged(IEnumerable<object> selectedObjects)
    {
        // Refresh the items to update their selected states
        Rebuild();
    }*/
    public void OnDataChanged(TableData tableData)
    {
        _rowDatas.Clear();

        foreach (KeyValuePair<int,NodeData> nodeData in tableData.Table)
        {
            Vector2Int pos = Vector2IntConverter.IntToVec2(tableData.Size, nodeData.Key);
            string position = $"({pos.x}, {pos.y})";
            string tileType = nodeData.Value.TileType.ToString();
            string cookingPropertyType = nodeData.Value.CookingPropertyType.ToString();
            string tileId = tableData.LevelDataDictionary.GetValueOrDefault(nodeData.Key, "");
            string spawnNpc = (nodeData.Value.IsSpawnNPC) ? nodeData.Value.SpawnNpcType.ToString() : "";
            string spawnObject = (nodeData.Value.IsSpawnObject) ? nodeData.Value.SpawnObjectType.ToString() : "";
            bool playerPosition = (tableData.PlayerPosition == new Vector2Int(pos.x, pos.y)) ? true : false;
            

            RowData rowData = new RowData()
            {
                positionIndex = nodeData.Key,
                Position = position,
                TileType = tileType,
                CookingPropertyType = cookingPropertyType,
                TileId = tileId,
                SpawnNpc = spawnNpc,
                SpawnObject = spawnObject,
                PlayerPosition = playerPosition,
            };
            _rowDatas.Add(rowData);
        }
        
        Rebuild();
    }
}
