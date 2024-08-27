using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectPopup : MonoBehaviour, IUIBase
{
    public GameObject PrevUI;
    public StageManager StageManager;

    public ToggleGroup ToggleGroup;

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

    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
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
                stageCount = 4;
                break;
            case 2:
                stageCount = 3;
                break;
            case 3:
                stageCount = 1;
                break;
        }

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
