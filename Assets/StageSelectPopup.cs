using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageSelectPopupType
{
    ChapterParent,
    StageParent,
    Btn_ExitPopup,
    Btn_ExitStage
}
public class StageSelectPopup : MonoBehaviour, IUIBase
{
    private Dictionary<StageSelectPopupType, IUIElement> _elements = new Dictionary<StageSelectPopupType, IUIElement>();

    private void OnEnable()
    {
        (GetUIElement(StageSelectPopupType.ChapterParent) as MonoBehaviour)?.gameObject.SetActive(true);
        (GetUIElement(StageSelectPopupType.StageParent) as MonoBehaviour)?.gameObject.SetActive(false);
        (GetUIElement(StageSelectPopupType.Btn_ExitPopup) as MonoBehaviour)?.gameObject.SetActive(false);
        (GetUIElement(StageSelectPopupType.Btn_ExitStage) as MonoBehaviour)?.gameObject.SetActive(false);
    }

    private IUIElement GetUIElement(StageSelectPopupType type)
    {
        if (!_elements.TryGetValue(type, out IUIElement uiElement))
        {
            GameObject ui = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(type.ToString());
            _elements.Add(type, ui.GetComponent<IUIElement>());
        }
        return _elements[type];
    }
}
