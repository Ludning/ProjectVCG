using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapInfoGrid : VisualElement
{
    private Label _header;
    private ListView _listView;
    public MapInfoGrid()
    {
        SetStyle();
        CreateInfoGrid();
    }
    public void CreateInfoGrid()
    {
        List<string> items = new List<string>()
        {
            "aa",
            "bb",
            "cc",
            "dd",
            "ee"
        };
        //_header = new Label("hahaha");
        //_listView = new ListView();
        
        Func<VisualElement> makeItem = () => new Label();
        Action<VisualElement, int> bindItem = (e, i) => (e as Label).text = items[i];
        const int itemHeight = 16;
        _listView = new ListView(items, itemHeight, makeItem, bindItem);
        _listView.selectionType = SelectionType.Multiple;

        _listView.itemsChosen += objects => Debug.Log(objects);
        _listView.selectionChanged += objects => Debug.Log(objects);

        _listView.style.flexGrow = 1.0f;
    }
    private void SetStyle()
    {
        style.flexGrow = 1;
        style.backgroundColor = new Color(0, 1, 0, 0.1f);
    }

    public void Init(TableData data)
    {
        _listView.Clear();
        _listView.Add(new Label($"{data.PlayerPosition},  {data.PlayerDirection}"));
        _listView.Add(new Label($"{data.PlayerPosition},  {data.PlayerDirection}"));
        _listView.Add(new Label($"{data.PlayerPosition},  {data.PlayerDirection}"));
        _listView.Add(new Label($"{data.PlayerPosition},  {data.PlayerDirection}"));
        _listView.Add(new Label($"{data.PlayerPosition},  {data.PlayerDirection}"));
        _listView.Add(new Label($"{data.PlayerPosition},  {data.PlayerDirection}"));
        _listView.Add(new Label($"{data.PlayerPosition},  {data.PlayerDirection}"));
    }
}
