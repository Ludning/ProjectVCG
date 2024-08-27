using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MainUIType
{
    StageSelectPopup,
    StageClearPopup,
}
public class MainUI : MonoBehaviour
{
    private Dictionary<MainUIType, IUIBase> _elements = new Dictionary<MainUIType, IUIBase>();

    private void OnEnable()
    {
        (GetUIBase(MainUIType.StageSelectPopup) as MonoBehaviour)?.gameObject.SetActive(true);
        (GetUIBase(MainUIType.StageClearPopup) as MonoBehaviour)?.gameObject.SetActive(false);
    }

    private IUIBase GetUIBase(MainUIType type)
    {
        if (!_elements.TryGetValue(type, out IUIBase uiBase))
        {
            GameObject ui = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(type.ToString());
            _elements.Add(type, ui.GetComponent<IUIBase>());
        }
        return _elements[type];
    }
}
