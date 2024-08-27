using System.Collections.Generic;
using UIInterface;
using UnityEngine;

public enum StageSelectPopupType
{
    ChapterSelectUI,
    StageSelectUI,
    Btn_ExitPopupUI,
    Btn_ExitStageUI
}
public class StageSelectPopup : MonoBehaviour, IUIBase
{
    private Dictionary<StageSelectPopupType, IUIElement> _elements = new Dictionary<StageSelectPopupType, IUIElement>();
    
    private void OnEnable()
    {
        // GetUIElement<ChapterSelectPopup>(StageSelectPopupType.ChapterSelectUI)?.gameObject.SetActive(true);
        GetUIElement<StageSelectUI>(StageSelectPopupType.StageSelectUI)?.gameObject.SetActive(false);
        GetUIElement<Btn_ExitPopupUI>(StageSelectPopupType.Btn_ExitPopupUI)?.gameObject.SetActive(false);
        GetUIElement<Btn_ExitStageUI>(StageSelectPopupType.Btn_ExitStageUI)?.gameObject.SetActive(false);
    }

    public T GetUIElement<T>(StageSelectPopupType type) where T : IUIElement
    {
        return (T)_elements.GetValueOrDefault(type);
    }
}
