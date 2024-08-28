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
        if (GameManager.Instance.SelectedChapterIndex == 0)
            return;
        NextUI.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Init()
    {
        
    }
}
