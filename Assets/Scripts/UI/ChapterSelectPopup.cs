using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterSelectPopup : MonoBehaviour, IUIBase
{
    [SerializeField] private StageSelectPopup StageSelectPopup;
    public ToggleGroup ToggleGroup;
    public void OnClick_OK()
    {
        int chapter = GameManager.Instance.SelectedChapterIndex;
        int stage = GameManager.Instance.SelectedStageIndex;
        Debug.Log($"{chapter}, {stage}");
        
        if (GameManager.Instance.OutOfRange_ChapterIndex())
            return;
        StageSelectPopup.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
    {
        ClearChildUI();
    }
    public void Init()
    {
        GameObject buttonTogglePrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("ButtonToggle");
        
        GameManager.Instance.SelectedChapterIndex = -1;
        List<int> chapterIndexList = GameManager.Instance.GetChapterIndexList();
        
        foreach (var chapterIndex in chapterIndexList)
        {
            ToggleHandler handler = Instantiate(buttonTogglePrefab, ToggleGroup.transform).GetComponent<ToggleHandler>();
            handler.type = ValueType.Chapter;
            handler.Value = chapterIndex;
            handler.Context.text = $"{chapterIndex}";
            
            handler.toggle.onValueChanged.AddListener(handler.OnToggleValueChanged);
            handler.toggle.group = ToggleGroup;
        }
    }
    public void ClearChildUI()
    {
        foreach (Transform child in ToggleGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
