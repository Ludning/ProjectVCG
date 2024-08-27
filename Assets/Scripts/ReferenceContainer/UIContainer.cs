using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MainUIType
{
    ChapterSelectPopup,
    StageSelectPopup,
    StageClearPopup,
    NPCMessagePopup,
}
public class UIContainer : MonoBehaviour
{
    private Dictionary<MainUIType, IUIBase> _elements = new Dictionary<MainUIType, IUIBase>();

    private void OnEnable()
    {
        GetUIBase<ChapterSelectPopup>(MainUIType.ChapterSelectPopup)?.gameObject.SetActive(true);
        GetUIBase<StageSelectPopup>(MainUIType.StageSelectPopup)?.gameObject.SetActive(false);
        GetUIBase<StageClearPopup>(MainUIType.StageClearPopup)?.gameObject.SetActive(false);
        GetUIBase<NPCMessagePopup>(MainUIType.NPCMessagePopup)?.gameObject.SetActive(false);
    }

    private T GetUIBase<T>(MainUIType type) where T : IUIBase
    {
        if (!_elements.TryGetValue(type, out IUIBase uiBase))
        {
            GameObject ui = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(typeof(T).ToString());
            _elements.Add(type, ui.GetComponent<IUIBase>());
        }
        return (T)_elements[type];
    }
}
