using System.Collections.Generic;
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

    public List<StageData> StageDatas = new List<StageData>();

    public void OnClick_OK()
    {
        if (GameManager.Instance.SelectedStageIndex == 0)
            return;
        if (StageDatas == null || StageDatas.Count == 0)
            return;
        
        int stageIndex = GameManager.Instance.SelectedStageIndex;

        StageData stageData = GameManager.Instance.GetStage(StageDatas, stageIndex);
        
        StageManager.InitStage(stageData);
        
        this.gameObject.SetActive(false);
    }
    public void OnClick_Back()
    {
        StageDatas.Clear();
        
        GameManager.Instance.SelectedStageIndex = 0;
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
        StageDatas = GameManager.Instance.GetChapterStageList(chapterIndex);
        
        switch (chapterIndex)
        {
            case 1:
                PrevChapterBtn.gameObject.SetActive(false);
                break;
            case 2:
                PrevChapterBtn.gameObject.SetActive(true);
                NextChapterBtn.gameObject.SetActive(true);
                break;
            case 3:
                NextChapterBtn.gameObject.SetActive(false);
                break;
        }
        ChapterText.text = $"{chapterIndex}챕터 \n 스테이지 선택";

        foreach (var stageData in StageDatas)
        {
            ToggleHandler handler = Instantiate(buttonTogglePrefab, ToggleGroup.transform).GetComponent<ToggleHandler>();
            handler.type = ValueType.Stage;
            handler.Value = stageData.Stage;
            handler.Context.text = $"{stageData.Chapter} - {stageData.Stage}";
            
            handler.toggle.onValueChanged.AddListener(handler.OnToggleValueChanged);
            handler.GetComponent<Toggle>().group = ToggleGroup;
        }
    }
}
