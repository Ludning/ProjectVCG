using UnityEngine;

public class ChapterSelectPopup : MonoBehaviour, IUIBase
{
    [SerializeField] private GameObject NextUI;
    public void OnSelectChapter(int value)
    {
        Debug.Log(value);
        GameManager.Instance.SelectedChapterIndex = value;
    }
    public void OnClick_OK()
    {
        if (GameManager.Instance.FilteredList == null)
            return;
        NextUI.SetActive(true);
        gameObject.SetActive(false);
        
        Debug.Log(GameManager.Instance.SelectedChapterIndex);
        Debug.Log(GameManager.Instance.SelectedStageIndex);
    }

    public void Init()
    {
        
    }
}
