using UnityEngine;
using UnityEngine.UIElements;

public class Inspector : VisualElement
{
    public Inspector()
    {
        SetStyle();
        SetInspector(null);
    }
    private void SetStyle()
    {
        style.flexGrow = 1;
        style.backgroundColor = new Color(0, 0, 1, 0.1f);  // 연한 파란색
    }

    public void SetInspector(MapNode mapNode)
    {
        TextField temp = new TextField("Name");
        Add(temp);
    }
}
