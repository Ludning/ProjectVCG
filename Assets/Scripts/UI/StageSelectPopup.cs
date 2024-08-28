using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectPopup : MonoBehaviour, IUIBase
{
    public GameObject PrevUI;
    public StageManager StageManager;

    public ToggleGroup ToggleGroup;
    public TextMeshProUGUI ChapterText;
    public Button PrevChapterBtn;
    public Button NextChapterBtn;

    public void OnSelectStage(int value)
    {
        GameManager.Instance.SelectedStageIndex = value;
    }
    public void OnClick_OK()
    {
        if (GameManager.Instance.SelectedStageIndex == 0)
            return;
        
        StageManager.InitStage();
        
        this.gameObject.SetActive(false);
    }
    public void OnClick_Back()
    {
        PrevUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void OnClick_PrevChapter()
    {
        ClearChildUI();
        GameManager.Instance.SelectedChapterIndex--;
        Init();
    }
    public void OnClick_NextChapter()
    {
        ClearChildUI();
        GameManager.Instance.SelectedChapterIndex++;
        Init();
    }
    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
    {
        ClearChildUI();
    }

    public void ClearChildUI()
    {
        foreach (Transform child in ToggleGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void Init()
    {
        GameObject buttonTogglePrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("ButtonToggle");
        int chapterIndex = GameManager.Instance.SelectedChapterIndex;

        int stageCount = 0;
        
        switch (chapterIndex)
        {
            case 1:
                PrevChapterBtn.gameObject.SetActive(false);
                stageCount = 4;
                break;
            case 2:
                PrevChapterBtn.gameObject.SetActive(true);
                NextChapterBtn.gameObject.SetActive(true);
                stageCount = 3;
                break;
            case 3:
                NextChapterBtn.gameObject.SetActive(false);
                stageCount = 1;
                break;
        }
        ChapterText.text = $"{chapterIndex}챕터 \n 스테이지 선택";

        for (int i = 1; i <= stageCount; i++)
        {
            ToggleHandler handler = Instantiate(buttonTogglePrefab, ToggleGroup.transform).GetComponent<ToggleHandler>();
            handler.type = ValueType.Stage;
            handler.Value = i;
            handler.Context.text = $"{GameManager.Instance.SelectedChapterIndex} - {i}";

            handler.GetComponent<Toggle>().group = ToggleGroup;
        }
        
    }
}
