using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ListViewExampleWindow : EditorWindow
{
    [MenuItem("Map Editor/ListViewExampleWindow")]
    public static void OpenDemoManual()
    {
        GetWindow<ListViewExampleWindow>().Show();
    }

    public void OnEnable()
    {
        // Create a list of data. In this case, numbers from 1 to 1000.
        const int itemCount = 1000;
        var items = new List<string>(itemCount);
        for (int i = 1; i <= itemCount; i++)
            items.Add(i.ToString());

        // The "makeItem" function is called when the
        // ListView needs more items to render.
        Func<VisualElement> makeItem = () => new Label();

        // As the user scrolls through the list, the ListView object
        // recycles elements created by the "makeItem" function,
        // and invoke the "bindItem" callback to associate
        // the element with the matching data item (specified as an index in the list).
        Action<VisualElement, int> bindItem = (e, i) => (e as Label).text = items[i];

        // Provide the list view with an explict height for every row
        // so it can calculate how many items to actually display
        const int itemHeight = 16;

        var listView = new UnityEngine.UIElements.ListView(items, itemHeight, makeItem, bindItem);

        listView.selectionType = SelectionType.Multiple;

        listView.itemsChosen += objects => Debug.Log(objects);
        listView.selectionChanged += objects => Debug.Log(objects);

        listView.style.flexGrow = 1.0f;

        rootVisualElement.Add(listView);
    }
}