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

    public void OnClick_OK()
    {
        int chapter = GameManager.Instance.SelectedChapterIndex;
        int stage = GameManager.Instance.SelectedStageIndex;
        Debug.Log($"{chapter}, {stage}");
        
        if (GameManager.Instance.OutOfRange_StageIndex(chapter))
            return;
        
        StageManager.InitStage();
        
        this.gameObject.SetActive(false);
    }
    public void OnClick_Back()
    {
        GameManager.Instance.SelectedStageIndex = -1;
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
        //StageDatas = GameManager.Instance.GetChapterStageList(chapterIndex);

        Debug.Log($"chapterIndex = {chapterIndex}");
        Debug.Log($"firstChapterIndex = {GameManager.Instance.firstChapterIndex}");
        Debug.Log($"lastChapterIndex = {GameManager.Instance.lastChapterIndex}");
        
        if (chapterIndex == GameManager.Instance.firstChapterIndex)
        {
            PrevChapterBtn.gameObject.SetActive(false);
            NextChapterBtn.gameObject.SetActive(true);
        }
        else if (chapterIndex == GameManager.Instance.firstChapterIndex)
        {
            PrevChapterBtn.gameObject.SetActive(true);
            NextChapterBtn.gameObject.SetActive(false);
        }
        else
        {
            PrevChapterBtn.gameObject.SetActive(true);
            NextChapterBtn.gameObject.SetActive(true);
        }
        
        /*switch (chapterIndex)
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
        }*/
        ChapterText.text = $"{chapterIndex}챕터 \n 스테이지 선택";

        List<int> stageIndexList = GameManager.Instance.GetStageIndexList(chapterIndex);
        
        foreach (var stageIndex in stageIndexList)
        {
            ToggleHandler handler = Instantiate(buttonTogglePrefab, ToggleGroup.transform).GetComponent<ToggleHandler>();
            handler.type = ValueType.Stage;
            handler.Value = stageIndex;
            handler.Context.text = $"{chapterIndex} - {stageIndex}";
            
            handler.toggle.onValueChanged.AddListener(handler.OnToggleValueChanged);
            handler.toggle.group = ToggleGroup;
        }
    }
}
