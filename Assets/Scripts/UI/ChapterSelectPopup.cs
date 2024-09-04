using UnityEngine;

public class ChapterSelectPopup : MonoBehaviour, IUIBase
{
    [SerializeField] private StageSelectPopup StageSelectPopup;
    public void OnClick_OK()
    {
        if (GameManager.Instance.SelectedChapterIndex == 0)
            return;
        StageSelectPopup.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Init()
    {
        GameManager.Instance.SelectedChapterIndex = 0;
    }
}
